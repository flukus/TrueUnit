﻿using System;
using AAATest;
using AAATest.ExampleProj;
using AAATest.ExampleProj.Dependencies;

namespace AAATest.ExampleTests {

    public interface IUseGlobalStub<T>
    {

    }

	class ProductController_Edit2 : TestFixture<ProductController>,
        IUseGlobalStub<Stubs.CommonStubs> ,
        IUseGlobalStub<Stubs.ProductRepository> {

		public void ExceptionWhenId0() {
			Act(x => x.Edit(0));
			AssertException<ArgumentException>("id must be provided. Provided value was: '0'");
		}

		public void ExceptionWhenIdNegative() {
			Act(x => x.Edit(-8922));
			AssertException<ArgumentException>("id must be provided. Provided value was: '-8922'");
		}

		public void ProductLoadedFromRepository(Stubs.ProductRepository products) {
			Act(x => x.Edit(27));
            Assert(products.GetById);
		}

		public void ExceptionWhenUnknownId() {
			Arrange((IRepository x) => x.GetById<Product>(99))
				.Returns(null);
			Act(x => x.Edit(99));
			AssertException("Unable to find product with id: '99'");
		}

		public void ResultFromReturnedObject(Stubs.ProductRepository products) {
            Arrange(products.GetById, x => { x.Id = 76; x.Name = "Super Awesome Gizmo"; });
			Act(x => x.Edit(76));
			Assert<ViewResult, ProductEditVM>(x => x.DataItem)
				.Equal(x => x.ProductName, "Super Awesome Gizmo")
				.Equal(x => x.ProductId, 76);
		}

		public void CategoryInfoIsSet(Stubs.ProductRepository products) {
            Arrange(products.FirstOrDefault, x=> x.Category =  new Category { Id = 3, Name = "foo" });
			Act(x => x.Edit(34));
			Assert<ViewResult, ProductEditVM>(x => x.DataItem)
				.Equal(x => x.CategoryId, 3)
				.Equal(x => x.CategoryName, "foo");
		}

		public void CategoryNullIfUnknown(Stubs.ProductRepository products) {
            Arrange(products.FirstOrDefault, x=> x.Category =  null);
			Act(x => x.Edit(34));
			Assert<ViewResult, ProductEditVM>(x => x.DataItem)
				.Null(x => x.CategoryId)
				.Null(x => x.CategoryName);
		}


		public void AvoidsLazyLoadingCategory(Stubs.ProductRepository products) {
			Act(x => x.Edit(31));
            Assert(products.IncludeCategory);
		}

        

	}
}