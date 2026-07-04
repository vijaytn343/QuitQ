import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getProductById, updateProduct } from "../api/productApi";
import { toast } from "react-toastify";
export default function EditProduct() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    subCategoryId: "",
    productName: "",
    description: "",
    price: "",
    brand: "",
    imageUrl: "",
    quantityAvailable: "",
    isActive: true
    
  });

  useEffect(() => {
    loadProduct();
  }, []);

  const loadProduct = async () => {
    try {
      const response = await getProductById(id);

      setFormData({
        subCategoryId: response.data.subCategoryId,
        productName: response.data.productName,
        description: response.data.description || "",
        price: response.data.price,
        brand: response.data.brand || "",
        imageUrl: response.data.imageUrl || "",
        quantityAvailable: response.data.quantityAvailable
      });
    } catch (error) {
      console.log(error);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await updateProduct(id, formData);

     toast.success("Product Updated Successfully");

      navigate("/my-products");
    } catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message ||
    error.response?.data ||
    error.message ||
    "Failed to update product"
  );
}
  };

  return (
    <div className="container mt-4">
      <h2>Edit Product</h2>
      <img
  src={formData.imageUrl}
  alt="Product"
  className="img-fluid mb-3"
  style={{
    maxHeight: "250px",
    objectFit: "contain"
  }}
/>

      <form onSubmit={handleSubmit}>
        <div className="card p-4">

         <label className="form-label">Product Name</label>
<input
  type="text"
  name="productName"
  className="form-control mb-3"
  value={formData.productName}
  onChange={handleChange}
/>

         <label className="form-label">Description</label>
<textarea
  name="description"
  className="form-control mb-3"
  value={formData.description}
  onChange={handleChange}
/>

         <label className="form-label">Price</label>
<input
  type="number"
  name="price"
  className="form-control mb-3"
  value={formData.price}
  onChange={handleChange}
/>

          <label className="form-label">Brand</label>
<input
  type="text"
  name="brand"
  className="form-control mb-3"
  value={formData.brand}
  onChange={handleChange}
/>

          <label className="form-label">Image URL</label>
<input
  type="text"
  name="imageUrl"
  className="form-control mb-3"
  value={formData.imageUrl}
  onChange={handleChange}
/>

          <label className="form-label">Quantity Available</label>
<input
  type="number"
  name="quantityAvailable"
  className="form-control mb-3"
  value={formData.quantityAvailable}
  onChange={handleChange}
/>

          <button
            type="submit"
            className="btn btn-warning"
          >
            Update Product
          </button>

        </div>
      </form>
    </div>
  );
}