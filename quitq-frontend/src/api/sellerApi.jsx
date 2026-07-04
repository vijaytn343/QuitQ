import API from "./axiosConfig";

export const createSellerProfile = (data) =>
  API.post("/Seller", data);
export const getSellerProfile = () => {
  return API.get("/Seller/profile");
};

export const updateSellerProfile = (data) => {
  return API.put("/Seller/profile", data);
};
export const getSalesReport = () =>
  API.get("/Seller/sales-report");
export const getSalesSummary = () =>
  API.get("/Seller/sales-summary");