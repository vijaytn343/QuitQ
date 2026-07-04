import API from "./axiosConfig";

export const getCategories = () =>
  API.get("/Category");

export const createCategory = (data) =>
  API.post("/Category", data);

export const updateCategory = (
  id,
  data
) =>
  API.put(`/Category/${id}`, data);

export const deleteCategory = (id) =>
  API.delete(`/Category/${id}`);