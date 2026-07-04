import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Slider from "rc-slider";
import "rc-slider/assets/index.css";

import {
  getAllProducts,
  getFilteredProducts
} from "../api/productApi";

import { getCategories }
  from "../api/categoryApi";

import ProductCard
  from "../components/ProductCard";

export default function Products() {
  const [searchParams] =
  useSearchParams();

const keyword =
  searchParams.get("keyword");

const categoryFromUrl =
  searchParams.get("categoryId");

  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);

const [totalPages, setTotalPages] = useState(1);

const pageSize = 6;
  const [categories, setCategories] = useState([]);
  const [sortBy, setSortBy] = useState("");
const [brand, setBrand] = useState("");
const [brands, setBrands] = useState([]);

const [priceRange, setPriceRange] =
  useState([0, 100000]);
 const [categoryId, setCategoryId] =
  useState(categoryFromUrl || "");
  

  useEffect(() => {

  if (categoryFromUrl) {
    setCategoryId(categoryFromUrl);
  }

  loadProducts();
  loadCategories();

}, [keyword, categoryFromUrl]);
useEffect(() => {

    loadProducts();

}, [categoryId, brand, currentPage, sortBy]);
useEffect(() => {

  const uniqueBrands = [
    ...new Set(
      products
        .map(p => p.brand)
        .filter(Boolean)
    )
  ];

  setBrands(uniqueBrands.sort());

}, [products]);
  const loadCategories = async () => {
    try {

      const response =
        await getCategories();
console.log(response.data);
      setCategories(response.data);

    } catch (error) {
      console.log(error);
    }
  };

  const loadProducts = async () => {

    try {

      let response;

      if (
  keyword ||
  categoryId ||
  brand ||
  priceRange[0] > 0 ||
  priceRange[1] < 100000
) {
response =
await getFilteredProducts(
    keyword,
    priceRange[0],
    priceRange[1],
    categoryId,
    brand,
    sortBy,
    currentPage,
    pageSize
);

      } else {

       response =
await getAllProducts(
    currentPage,
    pageSize
);
      }

      setProducts(response.data.items);

setCurrentPage(
    response.data.currentPage
);

setTotalPages(
    response.data.totalPages
);
    } catch (error) {
      console.log(error);
    }
  };
  
console.log("URL Category:", categoryFromUrl);
console.log("State Category:", categoryId);
  return (
    <div className="container-fluid">

      <div className="row">

        {/* Filters */}
        <div className="col-md-3">

          <div className="card p-3">

            <h5>Filters</h5>

            {keyword && (
              <p>
                Search:
                <strong>
                  {" "}
                  {keyword}
                </strong>
              </p>
            )}

            <div className="mb-3">
              <label className="form-label">
                Category
              </label>

              <select
                className="form-select"
                value={categoryId}
               onChange={(e)=>{

setCategoryId(
    e.target.value
);

setCurrentPage(1);

}}
              >
                <option value="">
                  All Categories
                </option>

                {categories.map(
                  (category) => (
                    <option
                      key={
                        category.categoryId
                      }
                      value={
                        category.categoryId
                      }
                    >
                      {
                        category.categoryName
                      }
                    </option>
                  )
                )}
              </select>
            </div>
            <div className="mb-3">
  <label className="form-label">
    Brand
  </label>

  <select
    className="form-select"
    value={brand}
    onChange={(e) => setBrand(e.target.value)}
  >
    <option value="">All Brands</option>

    {brands.map(b => (
      <option key={b} value={b}>
        {b}
      </option>
    ))}

  </select>
</div>
            <div className="mb-3">
  <label className="form-label">
    Sort By
  </label>

 <select
  className="form-select"
  value={sortBy}
  onChange={(e) => {
    setSortBy(e.target.value);
    setCurrentPage(1);
  }}
>
    <option value="">
      Default
    </option>

    <option value="priceAsc">
      Price Low → High
    </option>

    <option value="priceDesc">
      Price High → Low
    </option>
  </select>
</div>

         <div className="mb-4">

  <label className="form-label">
    Price Range
  </label>

  <p>
    ₹{priceRange[0]} - ₹{priceRange[1]}
  </p>

  <Slider
    range
    min={0}
    max={100000}
    value={priceRange}
    onChange={(value) =>
      setPriceRange(value)
    }
  />

</div>

            <button
              className="btn btn-primary w-100"
              
             onClick={()=>{
    setCurrentPage(1);
    loadProducts();
}}
            >
              Apply Filter
            </button>
            <button
  className="btn btn-secondary w-100 mt-2"
  onClick={async () => {
    setCurrentPage(1);
    setCategoryId("");
    setPriceRange([0, 100000]);
    setSortBy("");
    setBrand("");

    const response = await getAllProducts(
    1,
    pageSize
);

setProducts(response.data.items);

setCurrentPage(
    response.data.currentPage
);

setTotalPages(
    response.data.totalPages
);
  }}
>
  Reset Filter
</button>

          </div>

        </div>

        {/* Products */}
        <div className="col-md-9">
          <p className="mb-3">
  Showing Page {currentPage} of {totalPages}
</p>

          <div className="row g-4">

            {products.length > 0 ? (

              products.map(
                (product) => (
                  <div
                    key={
                      product.productId
                    }
                    className="col-lg-4 col-md-6"
                  >
                    <ProductCard
                      product={product}
                    />
                  </div>
                )
              )

            ) : (

              <div className="col-12">
                <div className="alert alert-warning">
                  No products found.
                </div>
              </div>

            )}

          </div>
          <div className="d-flex justify-content-center align-items-center mt-4 gap-3">

<button
className="btn btn-outline-primary"
disabled={currentPage===1}
onClick={()=>
setCurrentPage(currentPage-1)
}
>
Previous
</button>

<span>

Page {currentPage}
of {totalPages}

</span>

<button
className="btn btn-outline-primary"
disabled={currentPage===totalPages}
onClick={()=>
setCurrentPage(currentPage+1)
}
>
Next
</button>

</div>

        </div>

      </div>

    </div>
  );
}