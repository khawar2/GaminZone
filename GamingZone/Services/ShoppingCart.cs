﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GamingZone.Models;
using GamingZone.ViewModels;

namespace GamingZone.Services
{
    public class ShoppingCart
    {
        private readonly GamingZoneEntities _db;
        private readonly string _cartId;
        private readonly HttpContextBase _httpContext;

        public ShoppingCart(HttpContextBase context) 
            : this(context, new GamingZoneEntities())
        {
        }

        public ShoppingCart(HttpContextBase httpContext, GamingZoneEntities storeContext)
        {
            _db = storeContext;
            _cartId = GetCartId(httpContext);
            _httpContext = httpContext;
        }

        public async Task AddAsync(int productId)
        {
            var product = await _db.Products
                .SingleOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId && c.UserId == id);

            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = _cartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    UserId = id
                };

                _db.CartItems.Add(cartItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(int productId)
        {
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId && c.UserId==id);

            var itemCount = 0;

            if (cartItem == null)
            {
                return itemCount;
            }

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                itemCount = cartItem.Count;
            }
            else
            {
                _db.CartItems.Remove(cartItem);
            }

            await _db.SaveChangesAsync();

            return itemCount;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            return await _db.CartItems.Include("Product")
                .Where(c => c.CartId == _cartId && c.UserId==id).ToArrayAsync();
        }

        public async Task<PaymentResult> CheckoutAsync(CheckoutViewModel model)
        {
            var items = await GetCartItemsAsync();
            var order = new Order()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                Email = model.Email,
                OrderDate = DateTime.Now
            };

            foreach (var item in items)
            {
                var detail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    UnitPrice = (decimal?)item.Product.UnitPrice,
                    Quantity = item.Count
                };

                order.Total += (decimal)((item.Product.UnitPrice * item.Count));

                order.OrderDetails.Add(detail);
            }

            model.Total = order.Total;

            var gateway = new PaymentGateway();
             var result = gateway.ProcessPayment(model);

            //if (result.Succeeded)
            //{
            order.TransactionId = "1w24etew";
                //result.TransactionId;
                _db.Orders.Add(order);
                _db.CartItems.RemoveRange(items);
                await _db.SaveChangesAsync();
            //}
            PaymentResult resultOut = new PaymentResult("1w24etew",true,"Payment Successfuly Done!");
            return resultOut;
        }

        private string GetCartId(HttpContextBase http)
        {
            
            var cookie = http.Request.Cookies.Get("ShoppingCart");
            var cartId = string.Empty;

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie = new HttpCookie("ShoppingCart");
                cartId = Guid.NewGuid().ToString();

                cookie.Value = cartId;
                cookie.Expires = DateTime.Now.AddDays(7);

                http.Response.Cookies.Add(cookie);
            }
            else
            {
                cartId = cookie.Value;
            }

            return cartId;
        }

    }
}