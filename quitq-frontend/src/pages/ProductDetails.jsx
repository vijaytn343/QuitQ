import { Button } from "react-bootstrap";
import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

import { addToCart } from "../api/cartApi";
import {
  getProductById,
  getRelatedProducts
} from "../api/productApi";
import { toast } from "react-toastify";

function ProductDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const role = localStorage.getItem("role");

  const [product, setProduct] = useState(null);
  const [relatedProducts, setRelatedProducts] = useState([]);

  useEffect(() => {
    window.scrollTo({
      top: 0,
      behavior: "smooth"
    });

    loadProduct();
    loadRelatedProducts();

  }, [id]);

  const loadProduct = async () => {
    try {
      const response = await getProductById(id);
      setProduct(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  const loadRelatedProducts = async () => {
    try {
      const response = await getRelatedProducts(id);
      setRelatedProducts(response.data);
    }
    catch (error) {
      console.log(error);
    }
  };

  const handleAddToCart = async () => {
    try {
      await addToCart({
        productId: product.productId,
        quantity: 1,
      });

      toast.success("Product Added To Cart");
    }
    catch (error) {
      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to add product to cart"
      );
    }
  };

  const handleBuyNow = async () => {
    try {
      await addToCart({
        productId: product.productId,
        quantity: 1
      });

      navigate("/checkout");
    }
    catch (error) {
      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Unable to proceed to checkout"
      );
    }
  };

  if (!product) {
    return <h3>Loading...</h3>;
  }

  return (
    <div className="container py-5">
      <div className="row">

        <div className="col-md-5">
          <div className="card shadow-sm p-3">
            <img
              src={
                product.imageUrl ||
                "https://via.placeholder.com/400"
              }
              alt={product.productName}
              className="img-fluid"
              style={{
                height: "400px",
                objectFit: "contain",
              }}
            />
          </div>
        </div>

        <div className="col-md-7">

          <h2>{product.productName}</h2>

          <p className="text-muted">
            Brand: {product.brand}
          </p>

          <h3 className="text-success">
            ₹{product.price}
          </h3>

          <hr />

          <p>
            <strong>Category:</strong>{" "}
            {product.categoryName}
          </p>

          <p>
            <strong>Sub Category:</strong>{" "}
            {product.subCategoryName}
          </p>

          <p>
            <strong>Stock:</strong>{" "}
            {product.quantityAvailable > 0 ? (
              <span className="text-success">
                In Stock ({product.quantityAvailable})
              </span>
            ) : (
              <span className="text-danger">
                Out Of Stock
              </span>
            )}
          </p>

          <p>
            <strong>Description:</strong>
          </p>

          <p>{product.description}</p>

          {/* Only Customers can purchase */}

         {role === "Customer" && (

  product.quantityAvailable > 0 ? (

    <div className="d-flex gap-3 mt-4">

      <Button
        variant="warning"
        onClick={handleAddToCart}
      >
        Add To Cart
      </Button>

      <Button
        variant="danger"
        onClick={handleBuyNow}
      >
        Buy Now
      </Button>

    </div>

  ) : (

    <div className="mt-4">

      <Button
        variant="secondary"
        className="w-100"
        disabled
      >
        Out Of Stock
      </Button>

    </div>

  )

)}
        </div>
      </div>

      {/* Related Products */}

      <div className="mt-5">

        <h3 className="mb-4">
          Related Products
        </h3>

        <div className="row">

          {relatedProducts.length > 0 ? (

            relatedProducts.map((item) => (

              <div
                key={item.productId}
                className="col-md-3 mb-4"
              >

                <div
                  className="card h-100 shadow-sm"
                  style={{
                    cursor:
                      item.quantityAvailable > 0
                        ? "pointer"
                        : "not-allowed",
                    opacity:
                      item.quantityAvailable > 0
                        ? 1
                        : 0.6
                  }}
                  onClick={() => {
                    if (item.quantityAvailable > 0) {
                      navigate(`/products/${item.productId}`);
                    }
                  }}
                >

                  <img
                    src={
                      item.imageUrl ||
                      "https://via.placeholder.com/250"
                    }
                    className="card-img-top"
                    alt={item.productName}
                    style={{
                      height: "180px",
                      objectFit: "contain"
                    }}
                  />

                  <div className="card-body">

                    <h6
                      className="card-title"
                      style={{
                        minHeight: "45px"
                      }}
                    >
                      {item.productName}
                    </h6>

                    <h5 className="text-success">
                      ₹{item.price}
                    </h5>

                    <p
                      className={
                        item.quantityAvailable > 0
                          ? "text-success fw-bold"
                          : "text-danger fw-bold"
                      }
                    >
                      {item.quantityAvailable > 0
                        ? `In Stock (${item.quantityAvailable})`
                        : "Out Of Stock"}
                    </p>

                    <button
                      className="btn btn-outline-primary btn-sm w-100"
                    >
                      View Details
                    </button>

                  </div>

                </div>

              </div>

            ))

          ) : (

            <p>No Related Products Found.</p>

          )}

        </div>

      </div>

    </div>
  );
}

export default ProductDetails;