import { Link, useLocation, useNavigate } from "react-router-dom";
import {
  FaChartPie,
  FaBoxOpen,
  FaPlusCircle,
  FaShoppingCart,
  FaChartLine,
  FaUser,
  FaSignOutAlt
} from "react-icons/fa";

export default function SellerSidebar() {

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
      path: "/seller-dashboard",
      icon: <FaChartPie />
    },
    {
      name: "My Products",
      path: "/my-products",
      icon: <FaBoxOpen />
    },
    {
      name: "Add Product",
      path: "/add-product",
      icon: <FaPlusCircle />
    },
    {
      name: "Orders",
      path: "/seller-orders",
      icon: <FaShoppingCart />
    },
    {
      name: "Sales Report",
      path: "/sales-report",
      icon: <FaChartLine />
    },
    {
      name: "My Profile",
      path: "/seller-my-profile",
      icon: <FaUser />
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
        QuitQ
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
        Logout
      </button>

    </div>
  );
}