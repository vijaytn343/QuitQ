import API from "./axiosConfig";

export const createProduct = (data) =>
  API.post("/Product", data);

export const getMyProducts = () => {
  return API.get("/Product/my-products");
};

export const deleteProduct = (id) => {
  return API.delete(`/Product/${id}`);
};

export const getProductById = (id) => {
  return API.get(`/Product/${id}`);
};
export const getRelatedProducts = (id) => {
  return API.get(`/Product/${id}/related`);
};

export const updateProduct = (id, data) => {
  return API.put(`/Product/${id}`, data);
};
export const getAllProducts = (
  page = 1,
  pageSize = 6
) => {
  return API.get("/Product", {
    params: {
      page,
      pageSize
    }
  });
};

export const getFilteredProducts = (
  keyword,
  minPrice,
  maxPrice,
  categoryId,
  brand,
  sortBy,
  page = 1,
  pageSize = 6
) => {
  return API.get("/Product", {
    params: {
      keyword,
      minPrice,
      maxPrice,
      categoryId,
      brand,
      sortBy,
      page,
      pageSize
    }
  });

};