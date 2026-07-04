import API from "./axiosConfig";

export const getMyAddress = () => {
  return API.get("/Address/my-addresses");
};

export const createAddress = (data) => {
  return API.post("/Address", data);
};

export const updateAddress = (id, data) => {
  return API.put(`/Address/${id}`, data);
};

export const deleteAddress = (id) => {
  return API.delete(`/Address/${id}`);
};