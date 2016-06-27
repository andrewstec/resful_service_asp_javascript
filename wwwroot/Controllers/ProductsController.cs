using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestExercise;
using System.Web.Http.Cors;

namespace RestExercise.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        private FoodStore743Entities db = new FoodStore743Entities();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            FoodStore743Entities context = new FoodStore743Entities();
            context.Configuration.LazyLoadingEnabled = false;
            return context.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            FoodStore743Entities context = new FoodStore743Entities();
            context.Configuration.LazyLoadingEnabled = false;
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.productID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.productID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = product.productID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            
            //var invoices = from products in db.Invoices
            //               where products.Products.Any(p => p.productID == id)
            //               select products;

            if (product != null)
            {

                var invoices = from inv in db.Invoices
                               where inv.Products.Any(p => p.productID == product.productID)
                               select inv;

                foreach (var invoice in invoices)
                {
                    invoice.Products.Remove(product);
                }

                var purchaseOrders = from pur in db.PurchaseOrders
                                     where pur.Products.Any(p => p.productID == product.productID)
                                     select pur;

                foreach (var purchaseOrder in purchaseOrders)
                {
                    purchaseOrder.Products.Remove(product);
                }

                var invoicesWithQuantities = from invWQ in db.ProductInvoiceWithQuantities
                                             where invWQ.Product.productID == product.productID
                                             select invWQ;

                foreach (var invoiceWithQuantity in invoicesWithQuantities)
                {
                    db.ProductInvoiceWithQuantities.Remove(invoiceWithQuantity);
                }
            }

            //var productInvoicesWithQuantities = from products in db.Products
            //                                    from productInvoicesWQuantities in products.ProductInvoiceWithQuantities
            //                                    where productInvoicesWQuantities.productID == id
            //                                    select productInvoicesWQuantities;



            //IEnumerable<PurchaseOrder> productPurchaseOrder = db.PurchaseOrders.Where( ppo => ppo.Products.Any( o=> o.productID == id));

            //foreach ( Invoice invoice in invoices )
            //{
            //    db.Invoices.Remove(invoice);
            //}

            //foreach (ProductInvoiceWithQuantity productInvoice in productInvoices )
            //{
            //    db.ProductInvoiceWithQuantities.Remove(productInvoice);
            //}

            //foreach (PurchaseOrder ppo in productPurchaseOrder)
            //{
            //    db.PurchaseOrders.Remove(ppo);
            //}

            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.productID == id) > 0;
        }
    }
}