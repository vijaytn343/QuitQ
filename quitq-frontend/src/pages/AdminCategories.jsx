import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  getCategories,
  deleteCategory
} from "../api/categoryApi";

import { toast } from "react-toastify";

import AdminLayout from "../layouts/AdminLayout";

import {
  FaLayerGroup,
  FaSearch,
  FaTrash,
  FaPlus
} from "react-icons/fa";

export default function AdminCategories() {

  const [categories, setCategories] = useState([]);
  const [search, setSearch] = useState("");

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

  const handleDelete = async (id) => {

    const confirmDelete =
      window.confirm(
        "Delete this category?"
      );

    if (!confirmDelete) return;

    try {

      await deleteCategory(id);

      toast.success(
        "Category Deleted Successfully"
      );

      loadCategories();

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Unable to delete category"
      );

    }

  };

  const filteredCategories =
    categories.filter(c =>
      c.categoryName
        .toLowerCase()
        .includes(search.toLowerCase())
    );

  return (

    <AdminLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Manage Categories
          </h2>

          <p className="text-muted">
            Create and manage product categories.
          </p>

        </div>

        <div className="d-flex gap-3">

          <div className="card border-0 shadow-sm rounded-4 px-4 py-3">

            <h6 className="text-muted mb-1">
              Categories
            </h6>

            <h3 className="fw-bold text-primary">
              {categories.length}
            </h3>

          </div>

          <Link
            to="/admin-categories/add"
            className="btn btn-success d-flex align-items-center"
          >
            <FaPlus className="me-2" />
            Add Category
          </Link>

        </div>

      </div>

      {/* Search */}

      <div className="card border-0 shadow-sm rounded-4 mb-4">

        <div className="card-body">

          <div className="input-group">

            <span className="input-group-text bg-white">

              <FaSearch />

            </span>

            <input
              type="text"
              className="form-control"
              placeholder="Search category..."
              value={search}
              onChange={(e) =>
                setSearch(e.target.value)
              }
            />

          </div>

        </div>

      </div>

      {/* Table */}

      <div className="card border-0 shadow-sm rounded-4">

        <div className="card-body">

          <table className="table table-hover align-middle">

            <thead className="table-light">

              <tr>

                <th>ID</th>

                <th>Category</th>

                <th>Status</th>

                <th className="text-center">

                  Action

                </th>

              </tr>

            </thead>

            <tbody>

              {filteredCategories.length > 0 ? (

                filteredCategories.map(c => (

                  <tr key={c.categoryId}>

                    <td>

                      #{c.categoryId}

                    </td>

                    <td className="fw-semibold">

                      <FaLayerGroup className="me-2 text-primary" />

                      {c.categoryName}

                    </td>

                    <td>

                      <span className="badge bg-success">

                        Active

                      </span>

                    </td>

                    <td className="text-center">

                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() =>
                          handleDelete(c.categoryId)
                        }
                      >

                        <FaTrash className="me-2" />

                        Delete

                      </button>

                    </td>

                  </tr>

                ))

              ) : (

                <tr>

                  <td
                    colSpan="4"
                    className="text-center py-5"
                  >

                    <FaLayerGroup
                      size={40}
                      className="text-secondary mb-3"
                    />

                    <h5>

                      No Categories Found

                    </h5>

                  </td>

                </tr>

              )}

            </tbody>

          </table>

        </div>

      </div>

    </AdminLayout>

  );

}