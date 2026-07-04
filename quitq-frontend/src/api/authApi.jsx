import API from "./axiosConfig";

export const registerUser = (data) =>
  API.post("/Auth/register", data);

export const loginUser = (data) =>
  API.post("/Auth/login", data);