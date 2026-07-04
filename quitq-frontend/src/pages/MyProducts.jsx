import { useEffect, useState } from "react";
import { getMyProducts, deleteProduct } from "../api/productApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import SellerLayout from "../layouts/SellerLayout";
import { FaEdit, FaTrash } from "react-icons/fa";

export default function MyProducts() {
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const response = await getMyProducts();
      setProducts(response.data);
    } catch (error) {
      console.log(error);
      toast.error(
        error.response?.data?.message || "Failed to load products"
      );
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete this product?")) return;

    try {
      await deleteProduct(id);

      toast.success("Product Deleted Successfully");
      loadProducts();
    } catch (error) {
      console.log(error);

      toast.error(
        error.response?.data?.message || "Failed to delete product"
      );
    }
  };

  return (
    <SellerLayout>

      <h2 className="fw-bold mb-4">My Products</h2>

      <div className="row g-4">

        {products.map((product) => (

          <div
            key={product.productId}
            className="col-lg-4 col-md-6"
          >

            <div className="card border-0 shadow-sm rounded-4 h-100">

              <img
                src={product.imageUrl}
                alt={product.productName}
                className="card-img-top p-3"
                style={{
                  height: "230px",
                  objectFit: "contain"
                }}
              />

              <div className="card-body">

                <h5 className="fw-bold">
                  {product.productName}
                </h5>

                <h4 className="text-primary fw-bold">
                  ₹{product.price}
                </h4>

                <p className="text-muted mb-1">
                  Brand: <b>{product.brand}</b>
                </p>

                <p className="text-muted">
                  Stock: <b>{product.quantityAvailable}</b>
                </p>

              </div>

              <div className="card-footer bg-white border-0 d-flex gap-2">

                <button
                  className="btn btn-warning flex-fill"
                  onClick={() =>
                    navigate(`/edit-product/${product.productId}`)
                  }
                >
                  <FaEdit className="me-2" />
                  Edit
                </button>

                <button
                  className="btn btn-danger flex-fill"
                  onClick={() =>
                    handleDelete(product.productId)
                  }
                >
                  <FaTrash className="me-2" />
                  Delete
                </button>

              </div>

            </div>

          </div>

        ))}

      </div>

    </SellerLayout>
  );
}