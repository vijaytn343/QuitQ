import API from "./axiosConfig";

export const getSubCategories = () =>
  API.get("/SubCategory");

export const deleteSubCategory = (id) =>
  API.delete(`/SubCategory/${id}`);

export const createSubCategory = (data) =>
  API.post("/SubCategory", data);

export const getSubCategoryById = (id) =>
  API.get(`/SubCategory/${id}`);

export const updateSubCategory = (
  id,
  data
) =>
  API.put(
    `/SubCategory/${id}`,
    data
  );