import { useEffect, useState } from "react";
import { getSubCategories } from "../api/subCategoryApi";
import { createProduct } from "../api/productApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import SellerLayout from "../layouts/SellerLayout";

export default function AddProduct() {
  const navigate = useNavigate();

  const [subCategories, setSubCategories] = useState([]);

  const [formData, setFormData] = useState({
    subCategoryId: "",
    productName: "",
    description: "",
    price: "",
    brand: "",
    imageUrl: "",
    quantityAvailable: ""
  });

  useEffect(() => {
    loadSubCategories();
  }, []);

  const loadSubCategories = async () => {
    try {
      const response = await getSubCategories();
      setSubCategories(response.data);
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
      await createProduct(formData);

      toast.success("Product Added Successfully");

      navigate("/my-products");
    } catch (error) {
      console.log(error);

      toast.error(
        error.response?.data?.message ||
          "Failed to add product"
      );
    }
  };

  return (
    <SellerLayout>

      <h2 className="fw-bold mb-4">
        Add Product
      </h2>

      <div className="card border-0 shadow-sm rounded-4 p-4">

        <form onSubmit={handleSubmit}>

          <div className="row">

            <div className="col-md-6 mb-3">

              <label className="form-label fw-semibold">
                Sub Category
              </label>

              <select
                name="subCategoryId"
                className="form-select"
                value={formData.subCategoryId}
                onChange={handleChange}
              >
                <option value="">
                  Select Sub Category
                </option>

                {subCategories.map((sub) => (
                  <option
                    key={sub.subCategoryId}
                    value={sub.subCategoryId}
                  >
                    {sub.categoryName} - {sub.subCategoryName}
                  </option>
                ))}

              </select>

            </div>

            <div className="col-md-6 mb-3">

              <label className="form-label fw-semibold">
                Product Name
              </label>

              <input
                type="text"
                name="productName"
                className="form-control"
                value={formData.productName}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6 mb-3">

              <label className="form-label fw-semibold">
                Price
              </label>

              <input
                type="number"
                name="price"
                min="0.01"
                step="0.01"
                className="form-control"
                value={formData.price}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6 mb-3">

              <label className="form-label fw-semibold">
                Brand
              </label>

              <input
                type="text"
                name="brand"
                className="form-control"
                value={formData.brand}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-12 mb-3">

              <label className="form-label fw-semibold">
                Image URL
              </label>

              <input
                type="text"
                name="imageUrl"
                className="form-control"
                value={formData.imageUrl}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-12 mb-3">

              <label className="form-label fw-semibold">
                Description
              </label>

              <textarea
                rows="4"
                name="description"
                className="form-control"
                value={formData.description}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6 mb-4">

              <label className="form-label fw-semibold">
                Quantity Available
              </label>

              <input
                type="number"
                min="0"
                name="quantityAvailable"
                className="form-control"
                value={formData.quantityAvailable}
                onChange={handleChange}
              />

            </div>

          </div>

          <button
            type="submit"
            className="btn btn-primary px-5"
          >
            Add Product
          </button>

        </form>

      </div>

    </SellerLayout>
  );
}