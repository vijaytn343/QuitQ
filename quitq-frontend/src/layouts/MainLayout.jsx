import Navbar from "../components/Navbar";
import Footer from "../components/Footer";
import { useLocation } from "react-router-dom";

export default function MainLayout({ children }) {

  const location = useLocation();

  const sellerPages = [
    "/seller-dashboard",
    "/seller-profile",
    "/seller-my-profile",
    "/seller-edit-profile",
    "/my-products",
    "/add-product",
    "/seller-orders",
    "/sales-report",
    "/edit-product"
  ];

  const adminPages = [
    "/admin-dashboard",
    "/admin-users",
    "/admin-sellers",
    "/admin-categories",
    "/admin-subcategories",
    "/admin-categories/add",
    "/admin-subcategories/add",
    "/admin-subcategories/edit"
  ];

  const hideNavbar =
    sellerPages.some(page =>
      location.pathname.startsWith(page)
    ) ||
    adminPages.some(page =>
      location.pathname.startsWith(page)
    );

  return (
    <div className="d-flex flex-column min-vh-100">

   {!hideNavbar && <Navbar />}

<main className="flex-grow-1">
  {children}
</main>

{!hideNavbar && <Footer />}

    </div>
  );
}