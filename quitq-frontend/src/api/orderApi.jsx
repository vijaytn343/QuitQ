import API from "./axiosConfig";

export const createOrder = (data) => {
  return API.post("/Order", data);
};

export const getMyOrders = () => {
  return API.get("/Order/my-orders");
};

export const getOrderById = (id) => {
  return API.get(`/Order/${id}`);
};
export const getSellerDashboard = () => {
  return API.get("/Order/seller-dashboard");
};
export const downloadInvoice = (orderId) => {
  return API.get(
    `/Order/invoice/${orderId}`,
    {
      responseType: "blob"
    }
  );
};