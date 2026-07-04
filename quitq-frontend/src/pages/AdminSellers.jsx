import { useEffect, useState } from "react";
import {
  getSellers,
  deleteSeller
} from "../api/adminApi";

import { toast } from "react-toastify";
import AdminLayout from "../layouts/AdminLayout";

import {
  FaStore,
  FaSearch,
  FaTrash
} from "react-icons/fa";

export default function AdminSellers() {

  const [sellers, setSellers] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    loadSellers();
  }, []);

  const loadSellers = async () => {
    try {

      const response = await getSellers();

      setSellers(response.data);

    } catch (error) {

      console.log(error);

      toast.error("Failed to load sellers");

    }
  };

  const handleDelete = async (id) => {

    if (!window.confirm("Delete this seller?"))
      return;

    try {

      await deleteSeller(id);

      toast.success("Seller Deleted Successfully");

      loadSellers();

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Unable to delete seller"
      );

    }

  };

  const filteredSellers = sellers.filter(seller =>
    seller.storeName
      .toLowerCase()
      .includes(search.toLowerCase())
  );

  return (

    <AdminLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Manage Sellers
          </h2>

          <p className="text-muted">
            View and manage all registered sellers.
          </p>

        </div>

        <div className="card border-0 shadow-sm rounded-4 px-4 py-3">

          <h6 className="text-muted mb-1">
            Total Sellers
          </h6>

          <h3 className="fw-bold text-success">
            {sellers.length}
          </h3>

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
              placeholder="Search Store..."
              value={search}
              onChange={(e) =>
                setSearch(e.target.value)
              }
            />

          </div>

        </div>

      </div>

      {/* Sellers Table */}

      <div className="card border-0 shadow-sm rounded-4">

        <div className="card-body">

          <table className="table table-hover align-middle">

            <thead className="table-light">

              <tr>

                <th>ID</th>

                <th>Store Name</th>

                <th>Status</th>

                <th className="text-center">

                  Action

                </th>

              </tr>

            </thead>

            <tbody>

              {filteredSellers.length > 0 ? (

                filteredSellers.map((seller) => (

                  <tr key={seller.sellerId}>

                    <td>

                      #{seller.sellerId}

                    </td>

                    <td className="fw-semibold">

                      <FaStore className="me-2 text-success" />

                      {seller.storeName}

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
                          handleDelete(seller.sellerId)
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

                    <FaStore
                      size={40}
                      className="text-secondary mb-3"
                    />

                    <h5>

                      No Sellers Found

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