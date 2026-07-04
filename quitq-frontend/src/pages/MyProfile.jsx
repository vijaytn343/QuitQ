import { useEffect, useState } from "react";
import { getSellerProfile } from "../api/sellerApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import SellerLayout from "../layouts/SellerLayout";

import {
  FaStore,
  FaEnvelope,
  FaUniversity,
  FaIdCard,
  FaUser,
  FaCreditCard
} from "react-icons/fa";

export default function MyProfile() {

  const [profile, setProfile] = useState(null);

  const navigate = useNavigate();

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {

      const response = await getSellerProfile();

      setProfile(response.data);

    } catch (error) {

      console.log(error);

      toast.error(
        error.response?.data?.message ||
        "Failed to load profile"
      );

    }
  };

  if (!profile) {

    return (

      <SellerLayout>

        <div className="text-center mt-5">

          <div
            className="spinner-border text-primary"
          ></div>

          <h5 className="mt-3">
            Loading Profile...
          </h5>

        </div>

      </SellerLayout>

    );

  }

  return (

    <SellerLayout>

      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>

          <h2 className="fw-bold">
            Seller Profile
          </h2>

          <p className="text-muted">
            Manage your business information.
          </p>

        </div>

        <button
          className="btn btn-primary px-4"
          onClick={() =>
            navigate("/seller-edit-profile")
          }
        >
          Edit Profile
        </button>

      </div>

      <div className="card border-0 shadow rounded-4">

        <div className="card-body p-5">

          <div className="text-center mb-5">

            <div
              className="rounded-circle bg-primary text-white d-inline-flex justify-content-center align-items-center"
              style={{
                width: "90px",
                height: "90px",
                fontSize: "35px"
              }}
            >
              <FaStore />
            </div>

            <h3 className="mt-3 fw-bold">

              {profile.storeName}

            </h3>

            <p className="text-muted">

              Verified Seller

            </p>

          </div>

          <div className="row g-4">

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaStore className="me-2" />
                  Store Name
                </h6>

                <h5>
                  {profile.storeName}
                </h5>

              </div>

            </div>

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaIdCard className="me-2" />
                  GST Number
                </h6>

                <h5>
                  {profile.gstNumber}
                </h5>

              </div>

            </div>

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaEnvelope className="me-2" />
                  Business Email
                </h6>

                <h5>
                  {profile.businessEmail}
                </h5>

              </div>

            </div>

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaUser className="me-2" />
                  Account Holder
                </h6>

                <h5>
                  {profile.accountHolderName}
                </h5>

              </div>

            </div>

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaCreditCard className="me-2" />
                  Account Number
                </h6>

                <h5>
                  {profile.accountNumber}
                </h5>

              </div>

            </div>

            <div className="col-md-6">

              <div className="border rounded-4 p-3 h-100">

                <h6 className="text-muted">
                  <FaUniversity className="me-2" />
                  Bank Name
                </h6>

                <h5>
                  {profile.bankName}
                </h5>

              </div>

            </div>

            <div className="col-md-12">

              <div className="border rounded-4 p-3">

                <h6 className="text-muted">
                  IFSC Code
                </h6>

                <h5>
                  {profile.ifscCode}
                </h5>

              </div>

            </div>

          </div>

        </div>

      </div>

    </SellerLayout>

  );

}