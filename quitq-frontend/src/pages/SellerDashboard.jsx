import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  FaShoppingCart,
  FaBoxOpen,
  FaRupeeSign
} from "react-icons/fa";

import { getSellerDashboard } from "../api/orderApi";
import SellerLayout from "../layouts/SellerLayout";

export default function SellerDashboard() {
  const navigate = useNavigate();

  const [dashboard, setDashboard] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const role = localStorage.getItem("role");

    if (!token) {
      navigate("/login");
      return;
    }

    if (role !== "Seller") {
      navigate("/");
      return;
    }

    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const response = await getSellerDashboard();
      setDashboard(response.data);
    } catch (error) {
      console.log(error);

      if (error.response?.status === 403) {
        navigate("/");
      }
    }
  };

  return (
    <SellerLayout>

      <h2 className="fw-bold">Seller Dashboard</h2>

      <p className="text-muted mb-4">
        Welcome back,
        <span className="fw-bold">
          {" "}
          {localStorage.getItem("name")}
        </span>
      </p>

      {dashboard && (
        <div className="row g-4">

          <div className="col-md-4">
            <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

              <FaShoppingCart
                size={35}
                className="text-primary mb-3 mx-auto"
              />

              <h6 className="text-muted">
                Total Orders
              </h6>

              <h2 className="fw-bold">
                {dashboard.totalOrders}
              </h2>

            </div>
          </div>

          <div className="col-md-4">
            <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

              <FaBoxOpen
                size={35}
                className="text-success mb-3 mx-auto"
              />

              <h6 className="text-muted">
                Products Sold
              </h6>

              <h2 className="fw-bold">
                {dashboard.totalProductsSold}
              </h2>

            </div>
          </div>

          <div className="col-md-4">
            <div className="card border-0 shadow-sm rounded-4 p-4 text-center">

              <FaRupeeSign
                size={35}
                className="text-warning mb-3 mx-auto"
              />

              <h6 className="text-muted">
                Total Revenue
              </h6>

              <h2 className="fw-bold">
                ₹{dashboard.totalRevenue}
              </h2>

            </div>
          </div>

        </div>
      )}

      <div className="card border-0 shadow-sm rounded-4 p-4 mt-5">

        <h4 className="fw-bold mb-3">
          Business Overview
        </h4>

        <p className="text-muted mb-2">
          ✔ Track your sales performance.
        </p>

        <p className="text-muted mb-2">
          ✔ Manage your products from the sidebar.
        </p>

        <p className="text-muted mb-2">
          ✔ View and process customer orders.
        </p>

        <p className="text-muted">
          ✔ Monitor your revenue and business growth.
        </p>

      </div>

    </SellerLayout>
  );
}