import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { registerUser } from "../api/authApi";
import { toast } from "react-toastify";

import {
  FaUser,
  FaEnvelope,
  FaPhone,
  FaLock,
  FaShoppingBag,
  FaShieldAlt,
  FaTruck,
  FaStore
} from "react-icons/fa";

export default function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    phone: "",
    password: "",
    role: "Customer"
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Frontend Phone Validation
    if (!/^\d{10}$/.test(formData.phone)) {
      toast.error("Phone number must be exactly 10 digits");
      return;
    }

    try {
      await registerUser(formData);

      toast.success("Registration Successful");
      navigate("/login");

    } catch (error) {

      const errors = error.response?.data?.errors;

      if (errors) {
        Object.values(errors).flat().forEach(msg => toast.error(msg));
        return;
      }

      toast.error(
        error.response?.data?.message ||
        error.message ||
        "Registration Failed"
      );
    }
  };

  return (
  <div className="container-fluid py-5" style={{ minHeight: "90vh", background: "#f8fafc" }}>
    <div className="container">

      <div
        className="row bg-white shadow-lg rounded-4 overflow-hidden"
        style={{ minHeight: "700px" }}
      >

        {/* LEFT SIDE */}

        <div
          className="col-lg-5 text-white p-5 d-flex flex-column justify-content-center"
          style={{
            background: "linear-gradient(135deg,#2563EB,#7C3AED)"
          }}
        >

          <div className="mb-5">

           <h1 className="fw-bold display-5">

  <FaShoppingBag className="me-3" />

  QuitQ

</h1>

            <h2 className="fw-bold mt-5">
              Shop Smart.
              <br />
              Live Better.
            </h2>

            <p className="mt-4 fs-5">
              Join thousands of customers buying quality
              products from trusted sellers.
            </p>

          </div>

          <div className="mt-4">

            <div className="d-flex mb-4">
              <FaTruck size={24} className="me-3 mt-1" />
              <div>
                <h5>Fast Delivery</h5>
                <small>Get products delivered quickly.</small>
              </div>
            </div>

            <div className="d-flex mb-4">
              <FaShieldAlt size={24} className="me-3 mt-1" />
              <div>
                <h5>Secure Shopping</h5>
                <small>Your information stays protected.</small>
              </div>
            </div>

            <div className="d-flex">
              <FaStore size={24} className="me-3 mt-1" />
              <div>
                <h5>Trusted Sellers</h5>
                <small>Verified sellers with quality products.</small>
              </div>
            </div>

          </div>

        </div>

        {/* RIGHT SIDE */}

        <div className="col-lg-7 p-5">

          <h2 className="fw-bold mb-2">
            Create Account
          </h2>

          <p className="text-muted mb-4">
            Start your shopping journey with QuitQ.
          </p>

          <form onSubmit={handleSubmit}>

            <div className="row">

              <div className="col-md-6 mb-3">

                <div className="input-group">

                  <span className="input-group-text">
                    <FaUser />
                  </span>

                  <input
                    type="text"
                    name="name"
                    className="form-control"
                    placeholder="Full Name"
                    value={formData.name}
                    onChange={handleChange}
                    required
                  />

                </div>

              </div>

              <div className="col-md-6 mb-3">

                <div className="input-group">

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
                    required
                  />

                </div>

              </div>

              <div className="col-md-6 mb-3">

                <div className="input-group">

                  <span className="input-group-text">
                    <FaPhone />
                  </span>

                  <input
                    type="tel"
                    name="phone"
                    className="form-control"
                    placeholder="Phone Number"
                    value={formData.phone}
                    onChange={handleChange}
                    required
                  />

                </div>

              </div>

              <div className="col-md-6 mb-3">

                <div className="input-group">

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
                    required
                  />

                </div>

              </div>

              <div className="col-12 mb-4">

                <select
                  name="role"
                  className="form-select"
                  value={formData.role}
                  onChange={handleChange}
                >
                  <option value="Customer">Customer</option>
                  <option value="Seller">Seller</option>
                </select>

              </div>

            </div>

            <button
              className="btn w-100 text-white fw-bold py-3"
              style={{
                background:
                  "linear-gradient(90deg,#2563EB,#7C3AED)",
                border: "none",
                borderRadius: "12px"
              }}
            >
              Create Account
            </button>

          </form>

          <div className="text-center mt-4">

            Already have an account?

            <Link
              to="/login"
              className="fw-bold ms-2"
            >
              Login
            </Link>

          </div>

        </div>

      </div>

    </div>

  </div>
);
}