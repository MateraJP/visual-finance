using VisualFinanceiro.Negocios.Context;
using VisualFinanceiro.Negocios.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace VisualFinanceiro.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public abstract class BaseCrudController<T> : BaseController
        where T : Entity
    {
        protected readonly ControleContaContext db;
        protected int depth = 1;
        public BaseCrudController(ControleContaContext context)
        {
            db = context;
        }

        [HttpGet]
        public virtual IActionResult List()
        {
            var set = db.Set<T>();

            if (depth > 0)
            {
                var relationship = typeof(T).GetProperties().Where(p => p.PropertyType.BaseType == typeof(Entity));
                if (relationship.Any())
                {
                    var cumulator = set.Include<T>(relationship.FirstOrDefault().Name);
                    foreach (var property in relationship.Skip(1))
                    {
                        cumulator = cumulator.Include<T>(property.Name);
                    }

                    return ResponderJsonResult(cumulator.ToList());
                }
            }

            return ResponderJsonResult(set.ToList());
        }

        [HttpGet, Route("{id}")]
        public virtual IActionResult GetById(long id)
        {
            var set = db.Set<T>();

            if (depth > 0)
            {
                var relationship = typeof(T).GetProperties().Where(p => p.PropertyType.BaseType == typeof(Entity));
                if (relationship.Any())
                {
                    var cumulator = set.Include<T>(relationship.FirstOrDefault().Name);
                    foreach (var property in relationship.Skip(1))
                    {
                        cumulator = cumulator.Include<T>(property.Name);
                    }
                    var result = cumulator.Where(t => t.Id == id).ToList().FirstOrDefault();
                    return ResponderJsonResult(cumulator.Where(t => t.Id == id).ToList().FirstOrDefault());
                }
            }

            return ResponderJsonResult(set.Find(id));
        }

        [HttpPost]
        public virtual IActionResult Create(T entity)
        {
            if (!ValidaCreate(entity))
                return ResponderValidationException();

            var set = db.Set<T>();
            set.Add(entity);
            db.SaveChanges();
            return ResponderJsonResult(entity);
        }

        [HttpPut]
        public virtual IActionResult Update(T entity)
        {
            var set = db.Set<T>();

            var local = set.Find(getId(entity));
            if (local == null)
                return NotFound();

            if (!ValidaUpdate(entity, local))
                return ResponderValidationException();

            db.Entry(local).CurrentValues.SetValues(entity); //Update entire structure
            db.SaveChanges();
            return ResponderJsonResult(entity);
        }

        [HttpDelete, Route("{id}")]
        public virtual IActionResult Delete(long id)
        {
            var set = db.Set<T>();

            var local = set.Find(id);
            if (local == null)
                return NotFound();

            if (!ValidaDelete(local))
                return ResponderValidationException();

            set.Remove(local);
            db.SaveChanges();
            return Ok();
        }

        protected virtual bool ValidaCreate(T entity)
        {
            return ModelState.IsValid;
        }

        protected virtual bool ValidaUpdate(T @new, T old)
        {
            return ModelState.IsValid;
        }

        protected virtual bool ValidaDelete(T entity)
        {
            return ModelState.IsValid;
        }

        private object getId(T entity)
        {
            var type = typeof(T);
            var property = type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (property == null)
                throw new KeyNotFoundException("'KeyAttribute' must be defined at entityModel for BaseCrud(FindById, Update, Delete) operations");

            return property.GetValue(entity);
        }
    }
}
