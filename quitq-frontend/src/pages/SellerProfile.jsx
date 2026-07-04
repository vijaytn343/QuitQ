import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createSellerProfile } from "../api/sellerApi";
import { toast } from "react-toastify";

export default function SellerProfile() {
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

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await createSellerProfile(formData);

     toast.success("Profile Saved Successfully");

      navigate("/seller-dashboard");

    } catch (error) {
  console.log(error);
  console.log(error.response);

toast.error(
  error.response?.data?.message ||
  error.response?.data ||
  "Failed to save seller profile"
);
}
  };

  return (
    <div className="container mt-4">
      <h2>Seller Profile</h2>

      <form onSubmit={handleSubmit}>
        <div className="card p-4">

          <input
            type="text"
            name="storeName"
            className="form-control mb-3"
            placeholder="Store Name"
            value={formData.storeName}
            onChange={handleChange}
          />

          <input
            type="text"
            name="gstNumber"
            className="form-control mb-3"
            placeholder="GST Number"
            value={formData.gstNumber}
            onChange={handleChange}
          />

          <input
            type="email"
            name="businessEmail"
            className="form-control mb-3"
            placeholder="Business Email"
            value={formData.businessEmail}
            onChange={handleChange}
          />

          <input
            type="text"
            name="accountHolderName"
            className="form-control mb-3"
            placeholder="Account Holder Name"
            value={formData.accountHolderName}
            onChange={handleChange}
          />

          <input
            type="text"
            name="accountNumber"
            className="form-control mb-3"
            placeholder="Account Number"
            value={formData.accountNumber}
            onChange={handleChange}
          />

          <input
            type="text"
            name="ifscCode"
            className="form-control mb-3"
            placeholder="IFSC Code"
            value={formData.ifscCode}
            onChange={handleChange}
          />

          <input
            type="text"
            name="bankName"
            className="form-control mb-3"
            placeholder="Bank Name"
            value={formData.bankName}
            onChange={handleChange}
          />

          <button
            type="submit"
            className="btn btn-success"
          >
            Save Seller Profile
          </button>

        </div>
      </form>
    </div>
  );
}