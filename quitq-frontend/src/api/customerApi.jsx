import API from "./axiosConfig";

export const getCustomerProfile = () =>
  API.get("/User/profile");

export const updateCustomerProfile = (id, data) =>
  API.put(`/User/${id}`, data);