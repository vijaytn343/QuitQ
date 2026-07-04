import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { getCategories } from "../api/categoryApi";
import { createSubCategory } from "../api/subCategoryApi";

import { toast } from "react-toastify";

import AdminLayout from "../layouts/AdminLayout";

import {
  FaTags,
  FaPlusCircle
} from "react-icons/fa";

export default function AdminAddSubCategory() {

  const navigate = useNavigate();

  const [categories, setCategories] = useState([]);

  const [categoryId, setCategoryId] = useState("");

  const [subCategoryName, setSubCategoryName] = useState("");

  const [description, setDescription] = useState("");

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {

    try {

      const response = await getCategories();

      setCategories(response.data);

    } catch (error) {

      console.log(error);

      toast.error("Failed to load categories");

    }

  };

  const handleSubmit = async (e) => {

    e.preventDefault();

    try {

      await createSubCategory({

        categoryId: parseInt(categoryId),

        subCategoryName,

        description

      });

      toast.success("SubCategory Added Successfully");

      navigate("/admin-subcategories");

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed To Add SubCategory"
      );

    }

  };

  return (

    <AdminLayout>

      <div className="mb-4">

        <h2 className="fw-bold">

          Add Sub Category

        </h2>

        <p className="text-muted">

          Create a new sub category under an existing category.

        </p>

      </div>

      <div className="card border-0 shadow rounded-4 p-5">

        <div className="text-center mb-5">

          <div
            className="bg-primary rounded-circle d-inline-flex justify-content-center align-items-center"
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

            New Sub Category

          </h3>

        </div>

        <form onSubmit={handleSubmit}>

          <div className="mb-4">

            <label className="form-label fw-semibold">

              Category

            </label>

            <select
              className="form-select form-select-lg"
              value={categoryId}
              onChange={(e) =>
                setCategoryId(e.target.value)
              }
              required
            >

              <option value="">
                Select Category
              </option>

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
              className="form-control form-control-lg"
              placeholder="Enter Sub Category Name"
              value={subCategoryName}
              onChange={(e) =>
                setSubCategoryName(e.target.value)
              }
              required
            />

          </div>

          <div className="mb-4">

            <label className="form-label fw-semibold">

              Description

            </label>

            <textarea
              rows="4"
              className="form-control"
              placeholder="Enter Description"
              value={description}
              onChange={(e) =>
                setDescription(e.target.value)
              }
            />

          </div>

          <div className="d-flex gap-3">

            <button
              type="submit"
              className="btn btn-primary btn-lg px-4"
            >

              <FaPlusCircle className="me-2" />

              Create Sub Category

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