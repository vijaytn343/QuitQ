import { BrowserRouter, Routes, Route } from "react-router-dom";

import MainLayout from "./layouts/MainLayout";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Products from "./pages/Products";
import Cart from "./pages/Cart";
import ProductDetails from "./pages/ProductDetails";

import SellerProfile from "./pages/SellerProfile";
import SellerDashboard from "./pages/SellerDashboard";
import AddProduct from "./pages/AddProduct";
import MyProducts from "./pages/MyProducts";
import EditProduct from "./pages/EditProduct";
import SellerOrders from "./pages/SellerOrders";

import EditProfile from "./pages/EditProfile";
import MyProfile from "./pages/MyProfile";
import MyAddress from "./pages/MyAddress";
import AddAddress from "./pages/AddAddress";
import EditAddress from "./pages/EditAddress";

import Checkout from "./pages/Checkout";
import MyOrders from "./pages/MyOrders";

import ProtectedRoute from "./components/ProtectedRoute";
import AdminDashboard from "./pages/AdminDashboard";
import AdminUsers from "./pages/AdminUsers";
import AdminSellers from "./pages/AdminSellers";
import AdminCategories
from "./pages/AdminCategories";
import AdminSubCategories
from "./pages/AdminSubCategories";
import AdminAddCategory
from "./pages/AdminAddCategory";
import AdminAddSubCategory
from "./pages/AdminAddSubCategory";
import EditSubCategory
from "./pages/EditSubCategory";
import SalesReport
from "./pages/SalesReport";
import { useEffect } from "react";
import API from "./api/axiosConfig";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import CustomerProfile from "./pages/CustomerProfile";
import ResetPassword from "./pages/ResetPassword";
import ForgotPassword from "./pages/ForgotPassword";
import EditUserProfile from "./pages/EditUserProfile";
function App() {
  return (
    <BrowserRouter>
    <MainLayout>
        <Routes>

          {/* Public Routes */}
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
    path="/forgot-password"
    element={<ForgotPassword />}
/>
          <Route
    path="/reset-password"
    element={<ResetPassword />}
/>
          <Route path="/products" element={<Products />} />
          <Route
            path="/products/:id"
            element={<ProductDetails />}
          />

          {/* Customer Routes */}
          <Route
            path="/cart"
            element={
              <ProtectedRoute role="Customer">
                <Cart />
              </ProtectedRoute>
            }
          />

          <Route
            path="/checkout"
            element={
              <ProtectedRoute role="Customer">
                <Checkout />
              </ProtectedRoute>
            }
          />

          <Route
            path="/my-orders"
            element={
              <ProtectedRoute role="Customer">
                <MyOrders />
              </ProtectedRoute>
            }
          />

         <Route
  path="/seller-my-profile"
  element={
    <ProtectedRoute role="Seller">
      <MyProfile />
    </ProtectedRoute>
  }
/>

<Route
  path="/seller-edit-profile"
  element={
    <ProtectedRoute role="Seller">
      <EditProfile />
    </ProtectedRoute>
  }
/>

          <Route
            path="/my-address"
            element={
              <ProtectedRoute role="Customer">
                <MyAddress />
              </ProtectedRoute>
            }
          />
          <Route
    path="/my-profile"
    element={
        <ProtectedRoute role="Customer">
            <CustomerProfile />
        </ProtectedRoute>
    }
/>
<Route
  path="/edit-user-profile"
  element={
    <ProtectedRoute role="Customer">
      <EditUserProfile />
    </ProtectedRoute>
  }
/>

          <Route
            path="/add-address"
            element={
              <ProtectedRoute role="Customer">
                <AddAddress />
              </ProtectedRoute>
            }
          />

          <Route
            path="/edit-address/:id"
            element={
              <ProtectedRoute role="Customer">
                <EditAddress />
              </ProtectedRoute>
            }
          />

          {/* Seller Routes */}
          <Route
            path="/seller-profile"
            element={
              <ProtectedRoute role="Seller">
                <SellerProfile />
              </ProtectedRoute>
            }
          />

          <Route
            path="/seller-dashboard"
            element={
              <ProtectedRoute role="Seller">
                <SellerDashboard />
              </ProtectedRoute>
            }
          />

          <Route
            path="/add-product"
            element={
              <ProtectedRoute role="Seller">
                <AddProduct />
              </ProtectedRoute>
            }
          />

          <Route
            path="/my-products"
            element={
              <ProtectedRoute role="Seller">
                <MyProducts />
              </ProtectedRoute>
            }
          />

          <Route
            path="/edit-product/:id"
            element={
              <ProtectedRoute role="Seller">
                <EditProduct />
              </ProtectedRoute>
            }
          />

          <Route
            path="/seller-orders"
            element={
              <ProtectedRoute role="Seller">
                <SellerOrders />
              </ProtectedRoute>
            }
          /><Route
  path="/admin-dashboard"
  element={
    <ProtectedRoute role="Admin">
      <AdminDashboard />
    </ProtectedRoute>
  }
/>

<Route
  path="/admin-users"
  element={
    <ProtectedRoute role="Admin">
      <AdminUsers />
    </ProtectedRoute>
  }
/>

<Route
  path="/admin-sellers"
  element={
    <ProtectedRoute role="Admin">
      <AdminSellers />
    </ProtectedRoute>
  }
/>
<Route
 path="/admin-categories"
 element={
  <ProtectedRoute role="Admin">
   <AdminCategories />
  </ProtectedRoute>
 }
/>
<Route
  path="/admin-subcategories"
  element={
    <ProtectedRoute role="Admin">
      <AdminSubCategories />
    </ProtectedRoute>
  }
/>
<Route
  path="/admin-categories/add"
  element={
    <ProtectedRoute role="Admin">
      <AdminAddCategory />
    </ProtectedRoute>
  }
/>
<Route
  path="/admin-subcategories"
  element={
    <ProtectedRoute role="Admin">
      <AdminSubCategories />
    </ProtectedRoute>
  }
/>
<Route
  path="/admin-subcategories/add"
  element={
    <ProtectedRoute role="Admin">
      <AdminAddSubCategory />
    </ProtectedRoute>
  }
/>
<Route
  path="/admin-subcategories/edit/:id"
  element={
    <ProtectedRoute role="Admin">
      <EditSubCategory />
    </ProtectedRoute>
  }
/>
<Route
  path="/sales-report"
  element={
    <ProtectedRoute role="Seller">
      <SalesReport />
    </ProtectedRoute>
  }
/>
        </Routes>
        </MainLayout>
     
      <ToastContainer
    position="top-right"
    autoClose={2500}
    hideProgressBar={false}
    newestOnTop
    closeOnClick
/>
    </BrowserRouter>
  );
}

export default App;