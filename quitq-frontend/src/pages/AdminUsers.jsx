import { useEffect, useState } from "react";
import {
  getUsers,
  deleteUser
} from "../api/adminApi";

import { toast } from "react-toastify";
import AdminLayout from "../layouts/AdminLayout";

import {
  FaUsers,
  FaSearch,
  FaTrash
} from "react-icons/fa";

export default function AdminUsers() {

  const [users, setUsers] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    try {

      const response = await getUsers();

      setUsers(response.data);

    } catch (error) {

      console.log(error);

      toast.error("Failed to load users");

    }
  };

  const handleDelete = async (id) => {

    if (!window.confirm("Delete this user?"))
      return;

    try {

      await deleteUser(id);

      toast.success("User Deleted Successfully");

      loadUsers();

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Unable to delete user"
      );

    }

  };

  const filteredUsers = users.filter(user =>
    user.name.toLowerCase().includes(search.toLowerCase()) ||
    user.email.toLowerCase().includes(search.toLowerCase())
  );

  return (

    <AdminLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Manage Users
          </h2>

          <p className="text-muted">
            View and manage all registered customers.
          </p>

        </div>

        <div
          className="card border-0 shadow-sm rounded-4 px-4 py-3"
        >

          <h6 className="text-muted mb-1">
            Total Users
          </h6>

          <h3 className="fw-bold text-primary">
            {users.length}
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
              placeholder="Search by name or email..."
              value={search}
              onChange={(e) =>
                setSearch(e.target.value)
              }
            />

          </div>

        </div>

      </div>

      {/* Users Table */}

      <div className="card border-0 shadow-sm rounded-4">

        <div className="card-body">

          <table className="table table-hover align-middle">

            <thead className="table-light">

              <tr>

                <th>ID</th>

                <th>Name</th>

                <th>Email</th>

                <th>Status</th>

                <th className="text-center">
                  Action
                </th>

              </tr>

            </thead>

            <tbody>

              {filteredUsers.length > 0 ? (

                filteredUsers.map(user => (

                  <tr key={user.userId}>

                    <td>

                      #{user.userId}

                    </td>

                    <td className="fw-semibold">

                      {user.name}

                    </td>

                    <td>

                      {user.email}

                    </td>

                    <td>

                      <span className="badge bg-success">

                        Active

                      </span>

                    </td>

                    <td className="text-center">

                      <button
                        className="btn btn-sm btn-danger"
                        onClick={() =>
                          handleDelete(user.userId)
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
                    colSpan="5"
                    className="text-center py-5"
                  >

                    <FaUsers
                      size={40}
                      className="text-secondary mb-3"
                    />

                    <h5>

                      No Users Found

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