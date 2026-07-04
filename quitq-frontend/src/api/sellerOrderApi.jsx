import API from "./axiosConfig";

export const getSellerOrders = () => {
  return API.get("/Order/seller-orders");
};

export const getSellerDashboard = () => {
  return API.get("/Order/seller-dashboard");
};

export const updateOrderStatus = (orderId, status) => {
  return API.put(
    `/Order/${orderId}/status?status=${status}`
  );
};