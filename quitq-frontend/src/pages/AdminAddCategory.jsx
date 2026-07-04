import { useState } from "react";
import { createCategory } from "../api/categoryApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import AdminLayout from "../layouts/AdminLayout";

import {
  FaLayerGroup,
  FaPlusCircle
} from "react-icons/fa";

export default function AdminAddCategory() {

  const navigate = useNavigate();

  const [categoryName, setCategoryName] =
    useState("");

  const handleSubmit = async (e) => {

    e.preventDefault();

    try {

      await createCategory({
        categoryName
      });

      toast.success(
        "Category Added Successfully"
      );

      navigate("/admin-categories");

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to add category"
      );

    }

  };

  return (

    <AdminLayout>

      <div className="mb-4">

        <h2 className="fw-bold">
          Add Category
        </h2>

        <p className="text-muted">
          Create a new product category.
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

            <FaLayerGroup
              size={38}
              color="white"
            />

          </div>

          <h3 className="mt-3 fw-bold">

            New Category

          </h3>

        </div>

        <form onSubmit={handleSubmit}>

          <label className="form-label fw-semibold">

            Category Name

          </label>

          <input
            type="text"
            className="form-control form-control-lg mb-4"
            placeholder="Enter category name..."
            value={categoryName}
            onChange={(e) =>
              setCategoryName(e.target.value)
            }
            required
          />

          <div className="d-flex gap-3">

            <button
              type="submit"
              className="btn btn-primary btn-lg px-4"
            >

              <FaPlusCircle className="me-2" />

              Create Category

            </button>

            <button
              type="button"
              className="btn btn-outline-secondary btn-lg"
              onClick={() =>
                navigate("/admin-categories")
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