import { useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import API from "../api/axiosConfig";
import { toast } from "react-toastify";

function ResetPassword() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const token = searchParams.get("token");

  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (newPassword !== confirmPassword) {
      toast.error("Passwords do not match");
      return;
    }

    try {
      await API.post("/Auth/reset-password", {
        token,
        newPassword,
      });

      toast.success("Password reset successful");

      setTimeout(() => {
        navigate("/login");
      }, 2000);
    } catch (err) {
      toast.error(
        err.response?.data?.message || "Password reset failed"
      );
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "450px" }}>
      <h2 className="mb-4 text-center">Reset Password</h2>

      <form onSubmit={handleSubmit}>

        <div className="mb-3">
          <label>New Password</label>

          <input
            type="password"
            className="form-control"
            value={newPassword}
            onChange={(e) =>
              setNewPassword(e.target.value)
            }
            required
          />
        </div>

        <div className="mb-3">
          <label>Confirm Password</label>

          <input
            type="password"
            className="form-control"
            value={confirmPassword}
            onChange={(e) =>
              setConfirmPassword(e.target.value)
            }
            required
          />
        </div>

        <button
          className="btn btn-primary w-100"
        >
          Reset Password
        </button>

      </form>
    </div>
  );
}

export default ResetPassword;