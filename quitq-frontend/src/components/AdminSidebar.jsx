import { Link, useLocation, useNavigate } from "react-router-dom";
import {
  FaChartPie,
  FaUsers,
  FaStore,
  FaLayerGroup,
  FaTags,
  FaSignOutAlt
} from "react-icons/fa";

export default function AdminSidebar() {

  const location = useLocation();
  const navigate = useNavigate();

  const logout = () => {
    localStorage.clear();
    navigate("/login");
    window.location.reload();
  };

  const menu = [
    {
      name: "Dashboard",
      path: "/admin-dashboard",
      icon: <FaChartPie />
    },
    {
      name: "Users",
      path: "/admin-users",
      icon: <FaUsers />
    },
    {
      name: "Sellers",
      path: "/admin-sellers",
      icon: <FaStore />
    },
    {
      name: "Categories",
      path: "/admin-categories",
      icon: <FaLayerGroup />
    },
    {
      name: "Sub Categories",
      path: "/admin-subcategories",
      icon: <FaTags />
    }
  ];

  return (

    <div
      className="bg-dark text-white p-3"
      style={{
        width: "250px",
        minHeight: "100vh",
        position: "fixed",
        left: 0,
        top: 0
      }}
    >

      <h3 className="text-center mb-4">
        QuitQ Admin
      </h3>

      {menu.map(item => (

        <Link
          key={item.path}
          to={item.path}
          className={`d-flex align-items-center text-decoration-none p-3 rounded mb-2 ${
            location.pathname === item.path
              ? "bg-primary text-white"
              : "text-white"
          }`}
        >

          <span className="me-3">
            {item.icon}
          </span>

          {item.name}

        </Link>

      ))}

      <button
        className="btn btn-danger w-100 mt-4"
        onClick={logout}
      >
        <FaSignOutAlt className="me-2" />
        Logout
      </button>

    </div>

  );

}