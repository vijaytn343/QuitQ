import { useEffect, useState } from "react";
import {
  getCategories
} from "../api/categoryApi";

import {
  getSubCategoryById,
  updateSubCategory
} from "../api/subCategoryApi";

import {
  useParams,
  useNavigate
} from "react-router-dom";

import { toast } from "react-toastify";

import AdminLayout from "../layouts/AdminLayout";

import {
  FaTags,
  FaSave
} from "react-icons/fa";

export default function EditSubCategory() {

  const { id } = useParams();

  const navigate = useNavigate();

  const [categories, setCategories] = useState([]);

  const [formData, setFormData] = useState({
    categoryId: "",
    subCategoryName: "",
    description: ""
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {

    try {

      const categoriesResponse =
        await getCategories();

      setCategories(categoriesResponse.data);

      const subCategoryResponse =
        await getSubCategoryById(id);

      setFormData(subCategoryResponse.data);

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to load Sub Category"
      );

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

      await updateSubCategory(id, formData);

      toast.success(
        "SubCategory Updated Successfully"
      );

      navigate("/admin-subcategories");

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to update Sub Category"
      );

    }

  };

  return (

    <AdminLayout>

      <div className="mb-4">

        <h2 className="fw-bold">
          Edit Sub Category
        </h2>

        <p className="text-muted">
          Update sub category details.
        </p>

      </div>

      <div className="card border-0 shadow rounded-4 p-5">

        <div className="text-center mb-5">

          <div
            className="bg-warning rounded-circle d-inline-flex justify-content-center align-items-center"
            style={{
              width: "90px",
              height: "90px"
            }}
          >
            <FaTags
              size={38}
              color="white"
            />
          </div>

          <h3 className="mt-3 fw-bold">
            {formData.subCategoryName}
          </h3>

        </div>

        <form onSubmit={handleSubmit}>

          <div className="mb-4">

            <label className="form-label fw-semibold">
              Category
            </label>

            <select
              name="categoryId"
              className="form-select form-select-lg"
              value={formData.categoryId}
              onChange={handleChange}
            >

              {categories.map(c => (

                <option
                  key={c.categoryId}
                  value={c.categoryId}
                >
                  {c.categoryName}
                </option>

              ))}

            </select>

          </div>

          <div className="mb-4">

            <label className="form-label fw-semibold">
              Sub Category Name
            </label>

            <input
              type="text"
              name="subCategoryName"
              className="form-control form-control-lg"
              value={formData.subCategoryName}
              onChange={handleChange}
            />

          </div>

          <div className="mb-4">

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

          <div className="d-flex gap-3">

            <button
              type="submit"
              className="btn btn-primary btn-lg px-4"
            >

              <FaSave className="me-2" />

              Save Changes

            </button>

            <button
              type="button"
              className="btn btn-outline-secondary btn-lg"
              onClick={() =>
                navigate("/admin-subcategories")
              }
            >
              Cancel
            </button>

          </div>

        </form>

      </div>

    </AdminLayout>

  );

}