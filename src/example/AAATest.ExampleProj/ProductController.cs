﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAATest.ExampleProj.Dependencies;

namespace AAATest.ExampleProj {
	public class ProductController {

		private readonly IRepository Repository;
		private readonly ISession Session;

		public ProductController(IRepository repository, ISession session){
			Repository = repository;
			Session = session;
		}

		public ActionResult View(int id) {
			if (id <= 0)
				throw new ArgumentException(string.Format("id must be provided. Provided value was: '{0}'", id));

			var product = Repository.GetById<Product>(id);
			if (product == null)
				throw new Exception(string.Format("Unable to find product with id: '{0}'", id));

			var vm = new ProductViewVM() {
				ProductId = product.Id,
				ProductName = product.Name
			};

			return new ViewResult(vm);
		}


		public ActionResult Edit(int id) {
			if (id <= 0)
				throw new ArgumentException(string.Format("id must be provided. Provided value was: '{0}'", id));
			//var product = Repository.GetById<Product>(id);
			var product = Repository.GetById<Product>(id);
			if (product == null)
				throw new Exception(string.Format("Unable to find product with id: '{0}'", id));

			return new ViewResult() {
				DataItem = new ProductEditVM {
					ProductId = product.Id,
					ProductName = product.Name,
					CategoryId = product.Category != null ? product.Category.Id as int? : null,
					CategoryName = product.Category != null ? product.Category.Name : null
				}
			};
		}

		public ActionResult Edit2(int id) {
			if (id <= 0)
				throw new ArgumentException(string.Format("id must be provided. Provided value was: '{0}'", id));
			//var product = Repository.GetById<Product>(id);
			var product = Repository.Query<Product>()
				.Where(x => x.Id == id)
				.Include(x => x.Category)
				.First();
			if (product == null)
				throw new Exception(string.Format("Unable to find product with id: '{0}'", id));

			return new ViewResult() {
				DataItem = new ProductEditVM {
					ProductId = product.Id,
					ProductName = product.Name,
					CategoryId = product.Category != null ? product.Category.Id as int? : null,
					CategoryName = product.Category != null ? product.Category.Name : null
				}
			};
		}

		public ActionResult List() {
			return null;
		}

	}
}
