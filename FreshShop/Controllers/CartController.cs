using FreshShop.ApiIntergration;
using FreshShop.Utilities;
using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Catalog.Cart;
using FreshShop.ViewModels.Catalog.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreshShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartApiClient _cartApiClient;
        private readonly ICouponApiClient _couponApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly IAddressApiClient _addressApiClient;
        private readonly IProductApiClient _productApiClient;
        private string _culture = CultureInfo.CurrentCulture.Name;

        public CartController(ICartApiClient cartApiClient, ICouponApiClient couponApiClient, IUserApiClient userApiClient, IAddressApiClient addressApiClient, IProductApiClient productApiClient)
        {
            _cartApiClient = cartApiClient;
            _couponApiClient = couponApiClient;
            _userApiClient = userApiClient;
            _addressApiClient = addressApiClient;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageSizeMore, int pageSize = 1, int pageIndex = 1)
        {
            var request = new GetCartPagingRequest()
            {
                Keyword = keyword,
                LanguageId = _culture,
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                PageIndex = pageIndex,
                PageSize = pageSizeMore > pageSize ? pageSizeMore : pageSize
            };
            var result = await _cartApiClient.GetCartByUserId(request);
            if (result.IsSuccessed)
            {
                ViewBag.PageSizeMore = pageSizeMore > pageSize ? pageSizeMore : pageSize;
                return View(result.ResultObj);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddCart(int productId, int quantity)
        {
            var request = new CartCreateRequest()
            {
                LanguageId = _culture,
                ProductID = productId,
                Quantity = quantity,
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            var result = await _cartApiClient.Create(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                ViewBag.AddCartError = true;

                return RedirectToAction("Index", "Cart");
            }
        }

        public async Task<JsonResult> RemoveItem(string id)
        {
            var result = await _cartApiClient.RemoveItem(int.Parse(id));
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public async Task<JsonResult> DeleteCart()
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _cartApiClient.DeleteCart(userId);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status=true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }

        public async Task<IActionResult> UpdateCart(int id, int quantity)
        {
            var request = new CartUpdateRequest()
            {
                Id = id,
                Quantity = quantity
            };
            var result = await _cartApiClient.UpdateCart(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                TempData["UpdateCartError"] = true;
                return RedirectToAction("Index", "Cart");
            }
        }

        public async Task<JsonResult> ApplyCoupon(string code)
        {
            var coupon = await _couponApiClient.GetCouponByCode(code.Trim().ToUpper());
            if (coupon.IsSuccessed)
            {
                return Json(new
                {
                    status = true,
                    id = coupon.ResultObj.Id,
                    discount = coupon.ResultObj.Discount
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public JsonResult GetCheckout(List<CheckoutModel> cartList)
        {
            if (cartList.Count > 0)
            {
                return Json(new
                {
                    status = true,
                    listCheckout = cartList
                });
            }
            return Json(new
            {
                status = false
            });

        }

        public async Task<IActionResult> Checkout(string listCheckout, string couponRequest)
        {
            var checkoutObjs = JsonConvert.DeserializeObject<List<CheckoutModel>>(listCheckout);
            var couponObj = JsonConvert.DeserializeObject<GetCouponCheckoutRequest>(couponRequest);
            if (couponObj == null)
            {
                couponObj = new GetCouponCheckoutRequest()
                {
                    Id = 0,
                    Code = "",
                    Discount = 0
                };
            }
            var listCartCheckout = new List<CartViewModel>();
            decimal subtotal = 0;
            decimal discount = 0;
            foreach(var item in checkoutObjs)
            {
                var product = await _productApiClient.GetByIdClient(item.ProductId, _culture);
                var cartViewModel = new CartViewModel()
                {
                    ID = item.CartId,
                    ProductID = product.ResultObj.ID,
                    ProductName = product.ResultObj.Name,
                    Price = product.ResultObj.Price,
                    Discount = product.ResultObj.Discount.HasValue ? product.ResultObj.Discount.Value : 0,
                    Quantity = item.Quantity,                    
                };
                discount += product.ResultObj.Discount.HasValue ? product.ResultObj.Discount.Value : 0;
                subtotal += product.ResultObj.Price * item.Quantity;
                listCartCheckout.Add(cartViewModel);
            }
            ViewBag.Discount = (int)discount;
            ViewBag.SubTotal = (int)subtotal;
            ViewBag.CouponId = couponObj.Id;
            ViewBag.CouponDiscount = (int)couponObj.Discount;

            ViewBag.ListCartCheckout = listCartCheckout;

            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _userApiClient.GetById(userId);
            var address = await _addressApiClient.GetAllByUserId(userId);
            if (address.IsSuccessed && address.ResultObj.Count > 0)
            {
                foreach(var item in address.ResultObj)
                {
                    var province = await _addressApiClient.GetProvince(item.ProvinceId);
                    var district = await _addressApiClient.GetDistrict(item.DistrictId);
                    if(!province.IsSuccessed || !district.IsSuccessed)
                    {
                        TempData["AddressError"] = true;
                        return RedirectToAction("Index"); 
                    }
                    item.ProvinceName = province.ResultObj.Name;
                    item.DistrictName = district.ResultObj.Name;
                }
                ViewBag.ListAddress = address.ResultObj.Select(x => new SelectListItem()
                {
                    Text = x.Detail + " - " + x.DistrictName + " - " + x.ProvinceName,
                    Value = x.Detail + " - " + x.DistrictName + " - " + x.ProvinceName,
                    Selected =x.IsDefault==true
                });
            }
            else
            {
                ViewBag.ListAddress = address.ResultObj.DefaultIfEmpty(new AddressViewModel() { }).Select(x => new SelectListItem()
                {
                    Text = "Chưa có địa chỉ...",
                    Value = "",
                });              
            }

            var userCheckoutViewModel = new UserCheckoutViewModel()
            {
                UserId = user.ResultObj.Id,
                FirstName = user.ResultObj.FirstName,
                LastName = user.ResultObj.LastName,
                ShipEmail = user.ResultObj.Email,
                ShipPhone = user.ResultObj.PhoneNumber,
                CouponId=couponObj.Id,
                CouponDiscount=couponObj.Discount,
                Discount=discount,
                
            };
            return View(userCheckoutViewModel);
        }

        public async Task<IActionResult> Order(UserCheckoutViewModel model, string checkouts)
        {
            if (!ModelState.IsValid) return View(model);
            if (String.IsNullOrEmpty(model.ShipAddress)) return RedirectToAction("Address", "User");

            var listCheckout = JsonConvert.DeserializeObject<List<CartViewModel>>(checkouts);
            var listOrderDetail = new List<OrderDetailCreateRequest>();
            foreach(var item in listCheckout)
            {
                var orderDetail = new OrderDetailCreateRequest()
                {
                    CartID=item.ID,
                    Amount = item.Price,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                };
                listOrderDetail.Add(orderDetail);
            }


            if (int.Parse(model.ShippingType) == SystemConstants.ExpressDelivery)
            {
                model.ShippingCost = SystemConstants.ExpressDeliveryCost;
            }
            else if (int.Parse(model.ShippingType) == SystemConstants.BusinessDelivery)
            {
                model.ShippingCost = SystemConstants.BusinessDeliveryCost;
            }
            else
            {
                model.ShippingCost = 0;
            }

            var orderCreateRequest = new OrderCreateRequest()
            {
                ShipAddress = model.ShipAddress,
                ShippingCost = model.ShippingCost,
                CouponID = model.CouponId,
                CreatedDate=DateTime.Now,
                ShipEmail=model.ShipEmail,
                ShipName=model.LastName +" "+model.FirstName,
                ShipPhone=model.ShipPhone,
                Status=SystemConstants.DefaultStatusOrder,
                Total=model.Total-(model.Discount.HasValue?model.Discount.Value:0)-(model.CouponDiscount.HasValue?model.CouponDiscount.Value:0)+model.ShippingCost,
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value),              
            };

            var result = await _cartApiClient.CreateOrder(orderCreateRequest, listOrderDetail);
            if (result.IsSuccessed)
            {
                TempData["OrderSuccess"] = true;
                return RedirectToAction("Index", "Home");
            }

            TempData["OrderError"] = true;
            return RedirectToAction("Index"); 
        }
    }
}
