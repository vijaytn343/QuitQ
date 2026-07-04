import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { FaSearch, FaUser, FaShoppingCart } from "react-icons/fa";

export default function Navbar() {

  const [search, setSearch] = useState("");

  const navigate = useNavigate();

  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");
  const name = localStorage.getItem("name");

  const handleSearch = () => {
    if (search.trim()) {
      navigate(`/products?keyword=${search}`);
    }
  };

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
    window.location.reload();
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm">

      <div className="container">

        {/* Logo */}
        <Link
          className="navbar-brand fw-bold text-primary fs-3"
          to="/"
        >
          QuitQ
        </Link>

        {/* Search */}
        <div className="d-flex flex-grow-1 mx-4">

          <input
            type="text"
            className="form-control"
            placeholder="Search products..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />

          <button
            className="btn btn-primary ms-2"
            onClick={handleSearch}
          >
            <FaSearch />
          </button>

        </div>

        {/* Right Side */}
        <div className="d-flex align-items-center gap-4">

          <Link
            to="/products"
            className="text-dark text-decoration-none fw-semibold"
          >
            Products
          </Link>

         {/* Guest */}
{!token && (
  <>
    <Link
      to="/login"
      className="btn btn-outline-primary"
    >
      <FaUser className="me-1" />
      Login
    </Link>

    <Link
      to="/register"
      className="btn btn-outline-primary"
    >
      Register
    </Link>
  </>
)}

          {/* Customer */}
          {token && role === "Customer" && (
            <>
              <Link
                to="/cart"
                className="text-dark text-decoration-none"
              >
                <FaShoppingCart className="me-1" />
                Cart
              </Link>

              <div className="dropdown">

                <button
                  className="btn btn-outline-primary dropdown-toggle"
                  data-bs-toggle="dropdown"
                >
                  Hi, {name}
                </button>

                <ul className="dropdown-menu dropdown-menu-end">

                  <li>
                    <Link
                      className="dropdown-item"
                      to="/my-profile"
                    >
                      My Profile
                    </Link>
                  </li>

                  <li>
                    <Link
                      className="dropdown-item"
                      to="/my-orders"
                    >
                      My Orders
                    </Link>
                  </li>

                  <li>
                    <Link
                      className="dropdown-item"
                      to="/my-address"
                    >
                      My Addresses
                    </Link>
                  </li>

                  <li>
                    <hr className="dropdown-divider" />
                  </li>

                  <li>
                    <button
                      className="dropdown-item text-danger"
                      onClick={handleLogout}
                    >
                      Logout
                    </button>
                  </li>

                </ul>

              </div>
            </>
          )}

          {/* Seller */}
          {token && role === "Seller" && (
            <>
              <Link
                to="/seller-dashboard"
                className="text-dark text-decoration-none"
              >
                Dashboard
              </Link>

              <Link
                to="/my-products"
                className="text-dark text-decoration-none"
              >
                My Products
              </Link>

              <Link
                to="/seller-orders"
                className="text-dark text-decoration-none"
              >
                Orders
              </Link>

              <Link
                to="/sales-report"
                className="text-dark text-decoration-none"
              >
                Sales Report
              </Link>

              <span className="fw-bold">
                Hi, {name}
              </span>

              <button
                className="btn btn-outline-danger btn-sm"
                onClick={handleLogout}
              >
                Logout
              </button>
            </>
          )}

          {/* Admin */}
          {token && role === "Admin" && (
            <>
              <Link
                to="/admin-dashboard"
                className="text-dark text-decoration-none"
              >
                Dashboard
              </Link>

              <Link
                to="/admin-users"
                className="text-dark text-decoration-none"
              >
                Users
              </Link>

              <Link
                to="/admin-sellers"
                className="text-dark text-decoration-none"
              >
                Sellers
              </Link>

              <button
                className="btn btn-outline-danger btn-sm"
                onClick={handleLogout}
              >
                Logout
              </button>
            </>
          )}

        </div>

      </div>

    </nav>
  );
}