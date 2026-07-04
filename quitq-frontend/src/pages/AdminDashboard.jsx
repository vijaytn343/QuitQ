import { useEffect, useState } from "react";
import { getDashboard } from "../api/adminApi";

import {
  FaUsers,
  FaStore,
  FaLayerGroup,
  FaTags,
  FaChartLine
} from "react-icons/fa";

import AdminLayout from "../layouts/AdminLayout";

export default function AdminDashboard() {

  const [dashboard, setDashboard] = useState(null);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const response = await getDashboard();
      setDashboard(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  if (!dashboard) {
    return (
      <AdminLayout>

        <div className="text-center mt-5">

          <div className="spinner-border text-primary"></div>

          <h5 className="mt-3">
            Loading Dashboard...
          </h5>

        </div>

      </AdminLayout>
    );
  }

  return (

    <AdminLayout>

      <div className="mb-4">

        <h2 className="fw-bold">
          Admin Dashboard
        </h2>

        <p className="text-muted">
          Welcome back, Administrator
        </p>

      </div>

      <div className="row g-4">

        <div className="col-lg-3 col-md-6">

          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

            <FaUsers
              size={38}
              className="text-primary mx-auto mb-3"
            />

            <h6 className="text-muted">
              Total Users
            </h6>

            <h2 className="fw-bold">
              {dashboard.totalUsers}
            </h2>

          </div>

        </div>

        <div className="col-lg-3 col-md-6">

          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

            <FaStore
              size={38}
              className="text-success mx-auto mb-3"
            />

            <h6 className="text-muted">
              Total Sellers
            </h6>

            <h2 className="fw-bold">
              {dashboard.totalSellers}
            </h2>

          </div>

        </div>

        <div className="col-lg-3 col-md-6">

          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

            <FaLayerGroup
              size={38}
              className="text-warning mx-auto mb-3"
            />

            <h6 className="text-muted">
              Categories
            </h6>

            <h2 className="fw-bold">
              {dashboard.totalCategories}
            </h2>

          </div>

        </div>

        <div className="col-lg-3 col-md-6">

          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

            <FaTags
              size={38}
              className="text-danger mx-auto mb-3"
            />

            <h6 className="text-muted">
              Sub Categories
            </h6>

            <h2 className="fw-bold">
              {dashboard.totalSubCategories}
            </h2>

          </div>

        </div>

      </div>

      <div className="card border-0 shadow-sm rounded-4 mt-5">

        <div className="card-body p-4">

          <h4 className="fw-bold mb-3">

            <FaChartLine className="me-2" />

            Admin Overview

          </h4>

          <p className="text-muted">
            ✔ Manage customers from the Users section.
          </p>

          <p className="text-muted">
            ✔ Approve and manage sellers.
          </p>

          <p className="text-muted">
            ✔ Organize categories and subcategories.
          </p>

          <p className="text-muted">
            ✔ Monitor the overall marketplace.
          </p>

        </div>

      </div>

    </AdminLayout>

  );

}