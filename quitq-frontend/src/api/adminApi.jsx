import API from "./axiosConfig";

export const getDashboard = () =>
  API.get("/Admin/dashboard");

export const getUsers = () =>
  API.get("/Admin/users");

export const deleteUser = (id) =>
  API.delete(`/Admin/users/${id}`);

export const getSellers = () =>
  API.get("/Admin/sellers");

export const deleteSeller = (id) =>
  API.delete(`/Admin/sellers/${id}`);