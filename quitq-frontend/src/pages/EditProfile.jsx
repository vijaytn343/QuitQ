import { useEffect, useState } from "react";
import {
  getSellerProfile,
  updateSellerProfile
} from "../api/sellerApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import {
  FaStore,
  FaEnvelope,
  FaIdCard,
  FaUser,
  FaUniversity,
  FaCreditCard
} from "react-icons/fa";

import SellerLayout from "../layouts/SellerLayout";

export default function EditProfile() {

  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    storeName: "",
    gstNumber: "",
    businessEmail: "",
    accountHolderName: "",
    accountNumber: "",
    ifscCode: "",
    bankName: ""
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {

      const response = await getSellerProfile();

      setFormData({
        storeName: response.data.storeName || "",
        gstNumber: response.data.gstNumber || "",
        businessEmail: response.data.businessEmail || "",
        accountHolderName: response.data.accountHolderName || "",
        accountNumber: response.data.accountNumber || "",
        ifscCode: response.data.ifscCode || "",
        bankName: response.data.bankName || ""
      });

    } catch (error) {
      console.log(error);
      toast.error("Failed to load profile");
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {

    e.preventDefault();

    try {

      await updateSellerProfile(formData);

      toast.success("Profile Updated Successfully");

      navigate("/seller-my-profile");

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to update profile"
      );
    }
  };

  return (

    <SellerLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Edit Seller Profile
          </h2>

          <p className="text-muted mb-0">
            Update your business information.
          </p>

        </div>

      </div>

      <form onSubmit={handleSubmit}>

        <div className="card border-0 shadow rounded-4 p-5">

          <div className="text-center mb-5">

            <div
              className="bg-primary rounded-circle d-inline-flex justify-content-center align-items-center"
              style={{
                width: "90px",
                height: "90px"
              }}
            >
              <FaStore
                size={38}
                color="white"
              />
            </div>

            <h3 className="mt-3 fw-bold">
              {formData.storeName || "Store Name"}
            </h3>

          </div>

          <div className="row g-4">

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaStore className="me-2" />
                Store Name
              </label>

              <input
                type="text"
                name="storeName"
                className="form-control form-control-lg"
                value={formData.storeName}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaIdCard className="me-2" />
                GST Number
              </label>

              <input
                type="text"
                name="gstNumber"
                className="form-control form-control-lg"
                value={formData.gstNumber}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaEnvelope className="me-2" />
                Business Email
              </label>

              <input
                type="email"
                name="businessEmail"
                className="form-control form-control-lg"
                value={formData.businessEmail}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaUser className="me-2" />
                Account Holder
              </label>

              <input
                type="text"
                name="accountHolderName"
                className="form-control form-control-lg"
                value={formData.accountHolderName}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaCreditCard className="me-2" />
                Account Number
              </label>

              <input
                type="text"
                name="accountNumber"
                className="form-control form-control-lg"
                value={formData.accountNumber}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-6">

              <label className="form-label fw-semibold">
                <FaUniversity className="me-2" />
                IFSC Code
              </label>

              <input
                type="text"
                name="ifscCode"
                className="form-control form-control-lg"
                value={formData.ifscCode}
                onChange={handleChange}
              />

            </div>

            <div className="col-md-12">

              <label className="form-label fw-semibold">
                <FaUniversity className="me-2" />
                Bank Name
              </label>

              <input
                type="text"
                name="bankName"
                className="form-control form-control-lg"
                value={formData.bankName}
                onChange={handleChange}
              />

            </div>

          </div>

          <div className="mt-5 d-flex gap-3">

            <button
              type="submit"
              className="btn btn-primary btn-lg px-5"
            >
              Save Changes
            </button>

            <button
              type="button"
              className="btn btn-outline-secondary btn-lg"
              onClick={() => navigate("/seller-my-profile")}
            >
              Cancel
            </button>

          </div>

        </div>

      </form>

    </SellerLayout>
  );
}