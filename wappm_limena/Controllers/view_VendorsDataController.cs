using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wappm_limena.Models;

namespace wappm_limena.Controllers
{
    public class view_VendorsDataController : Controller
    {
        private DLI_PROEntities db = new DLI_PROEntities();

        // GET: view_VendorsData
        public ActionResult Index()
        {
            return View(db.view_VendorsData.ToList());
        }

        // GET: view_VendorsData/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_VendorsData view_VendorsData = db.view_VendorsData.Find(id);
            if (view_VendorsData == null)
            {
                return HttpNotFound();
            }
            return View(view_VendorsData);
        }

        // GET: view_VendorsData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: view_VendorsData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "No_FACTURA,FECHA_FACTURA,MES_FACTURA,ANO_FACTURA,NOMBRE_CLIENTE,PAIS,STATE,CODIGO_ARTICULO,DESCRIPCION_ARTICULO,FAMILIA,CANAL,MARCA,VENDEDOR,CANTIDAD_EN_CAJAS,VALOR_DE_VENTA_EN_US,COSTO_TOTAL_EN_US,CODIGO_CLIENTE")] view_VendorsData view_VendorsData)
        {
            if (ModelState.IsValid)
            {
                db.view_VendorsData.Add(view_VendorsData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(view_VendorsData);
        }

        // GET: view_VendorsData/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_VendorsData view_VendorsData = db.view_VendorsData.Find(id);
            if (view_VendorsData == null)
            {
                return HttpNotFound();
            }
            return View(view_VendorsData);
        }

        // POST: view_VendorsData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "No_FACTURA,FECHA_FACTURA,MES_FACTURA,ANO_FACTURA,NOMBRE_CLIENTE,PAIS,STATE,CODIGO_ARTICULO,DESCRIPCION_ARTICULO,FAMILIA,CANAL,MARCA,VENDEDOR,CANTIDAD_EN_CAJAS,VALOR_DE_VENTA_EN_US,COSTO_TOTAL_EN_US,CODIGO_CLIENTE")] view_VendorsData view_VendorsData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view_VendorsData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(view_VendorsData);
        }

        // GET: view_VendorsData/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_VendorsData view_VendorsData = db.view_VendorsData.Find(id);
            if (view_VendorsData == null)
            {
                return HttpNotFound();
            }
            return View(view_VendorsData);
        }

        // POST: view_VendorsData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            view_VendorsData view_VendorsData = db.view_VendorsData.Find(id);
            db.view_VendorsData.Remove(view_VendorsData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
