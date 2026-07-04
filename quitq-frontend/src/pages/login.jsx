import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { loginUser } from "../api/authApi";
import { getSellerProfile } from "../api/sellerApi";
import { toast } from "react-toastify";

import {
  FaEnvelope,
  FaLock,
  FaShieldAlt,
  FaTruck,
  FaStore
} from "react-icons/fa";

export default function Login() {

  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    email: "",
    password: ""
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {

      const response = await loginUser(formData);

      localStorage.setItem(
        "token",
        response.data.token
      );

      localStorage.setItem(
        "role",
        response.data.role
      );

      localStorage.setItem(
        "userId",
        response.data.userId
      );

      localStorage.setItem(
        "name",
        response.data.name
      );

      toast.success("Login Successful");

      if (response.data.role === "Seller") {

        try {

          // Check whether seller profile exists
          await getSellerProfile();

          navigate("/seller-dashboard");

        } catch {

          navigate("/seller-profile");

        }

      }
      else if (response.data.role === "Admin") {

        navigate("/admin-dashboard");

      }
      else {

        navigate("/");

      }

    } catch (error) {

      console.error(error);

      toast.error("Invalid Email or Password");

    }
  };

 return (
  <div
    className="container-fluid py-5"
    style={{
      minHeight: "90vh",
      background: "#f8fafc"
    }}
  >
    <div className="container">

      <div
        className="row bg-white shadow-lg rounded-4 overflow-hidden"
        style={{ minHeight: "650px" }}
      >

        {/* LEFT PANEL */}

        <div
          className="col-lg-5 text-white p-5 d-flex flex-column justify-content-center"
          style={{
            background: "linear-gradient(135deg,#2563EB,#7C3AED)"
          }}
        >

          <h1 className="fw-bold display-5 mb-5">
            QuitQ
          </h1>

          <h2 className="fw-bold">
            Welcome Back!
          </h2>

          <p className="mt-3 fs-5">
            Login to continue shopping with QuitQ.
          </p>

          <div className="mt-5">

            <div className="d-flex mb-4">
              <FaTruck size={24} className="me-3 mt-1" />
              <div>
                <h5>Fast Delivery</h5>
                <small>Quick delivery at your doorstep.</small>
              </div>
            </div>

            <div className="d-flex mb-4">
              <FaShieldAlt size={24} className="me-3 mt-1" />
              <div>
                <h5>Secure Shopping</h5>
                <small>Your data is always protected.</small>
              </div>
            </div>

            <div className="d-flex">
              <FaStore size={24} className="me-3 mt-1" />
              <div>
                <h5>Trusted Sellers</h5>
                <small>Verified quality products.</small>
              </div>
            </div>

          </div>

        </div>

        {/* RIGHT PANEL */}

        <div className="col-lg-7 p-5 d-flex flex-column justify-content-center">

          <h2 className="fw-bold mb-2">
            Login
          </h2>

          <p className="text-muted mb-4">
            Welcome back! Please login to your account.
          </p>

          <form onSubmit={handleSubmit} autoComplete="off">

            <div className="input-group mb-3">

              <span className="input-group-text">
                <FaEnvelope />
              </span>

              <input
                type="email"
                name="email"
                className="form-control"
                placeholder="Email"
                value={formData.email}
                onChange={handleChange}
              />

            </div>

            <div className="input-group mb-4">

              <span className="input-group-text">
                <FaLock />
              </span>

              <input
                type="password"
                name="password"
                className="form-control"
                placeholder="Password"
                value={formData.password}
                onChange={handleChange}
              />

            </div>

            <button
              type="submit"
              className="btn w-100 text-white fw-bold py-3"
              style={{
                background:
                  "linear-gradient(90deg,#2563EB,#7C3AED)",
                border: "none",
                borderRadius: "12px"
              }}
            >
              Login
            </button>

            <div className="text-center mt-4">

              <Link to="/forgot-password">
                Forgot Password?
              </Link>

            </div>

          </form>

          <div className="text-center mt-4">

            New User?

            <Link
              to="/register"
              className="fw-bold ms-2"
            >
              Register
            </Link>

          </div>

        </div>

      </div>

    </div>
  </div>
);
}