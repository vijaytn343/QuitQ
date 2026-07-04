import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  getSubCategories,
  deleteSubCategory
} from "../api/subCategoryApi";

import { toast } from "react-toastify";

import AdminLayout from "../layouts/AdminLayout";

import {
  FaTags,
  FaSearch,
  FaTrash,
  FaEdit,
  FaPlus
} from "react-icons/fa";

export default function AdminSubCategories() {

  const [subCategories, setSubCategories] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    loadSubCategories();
  }, []);

  const loadSubCategories = async () => {
    try {

      const response = await getSubCategories();

      setSubCategories(response.data);

    } catch (error) {

      console.log(error);

      toast.error("Failed to load Sub Categories");

    }
  };

  const handleDelete = async (id) => {

    if (!window.confirm("Delete this Sub Category?"))
      return;

    try {

      await deleteSubCategory(id);

      toast.success("SubCategory Deleted Successfully");

      loadSubCategories();

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Unable to delete SubCategory"
      );

    }

  };

  const filteredSubCategories =
    subCategories.filter(sc =>
      sc.subCategoryName
        .toLowerCase()
        .includes(search.toLowerCase()) ||
      sc.categoryName
        .toLowerCase()
        .includes(search.toLowerCase())
    );

  return (

    <AdminLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Manage Sub Categories
          </h2>

          <p className="text-muted">
            Organize products using sub categories.
          </p>

        </div>

        <div className="d-flex gap-3">

          <div className="card border-0 shadow-sm rounded-4 px-4 py-3">

            <h6 className="text-muted mb-1">
              Sub Categories
            </h6>

            <h3 className="fw-bold text-primary">
              {subCategories.length}
            </h3>

          </div>

          <Link
            to="/admin-subcategories/add"
            className="btn btn-success d-flex align-items-center"
          >
            <FaPlus className="me-2" />
            Add Sub Category
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
              placeholder="Search sub category..."
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

                <th>Sub Category</th>

                <th>Category</th>

                <th>Status</th>

                <th className="text-center">
                  Action
                </th>

              </tr>

            </thead>

            <tbody>

              {filteredSubCategories.length > 0 ? (

                filteredSubCategories.map(sc => (

                  <tr key={sc.subCategoryId}>

                    <td>

                      #{sc.subCategoryId}

                    </td>

                    <td className="fw-semibold">

                      <FaTags className="me-2 text-primary" />

                      {sc.subCategoryName}

                    </td>

                    <td>

                      {sc.categoryName}

                    </td>

                    <td>

                      <span className="badge bg-success">

                        Active

                      </span>

                    </td>

                    <td className="text-center">

                      <Link
                        to={`/admin-subcategories/edit/${sc.subCategoryId}`}
                        className="btn btn-warning btn-sm me-2"
                      >

                        <FaEdit className="me-1" />

                        Edit

                      </Link>

                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() =>
                          handleDelete(sc.subCategoryId)
                        }
                      >

                        <FaTrash className="me-1" />

                        Delete

                      </button>

                    </td>

                  </tr>

                ))

              ) : (

                <tr>

                  <td
                    colSpan="5"
                    className="text-center py-5"
                  >

                    <FaTags
                      size={40}
                      className="text-secondary mb-3"
                    />

                    <h5>
                      No Sub Categories Found
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