import { useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../api/axiosConfig";
import { toast } from "react-toastify";

function ForgotPassword() {
  const [email, setEmail] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await API.post("/Auth/forgot-password", {
        email,
      });

      toast.success("Password reset email sent.");
      navigate("/login");
    } catch {
      toast.error("Something went wrong.");
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "450px" }}>
      <h2 className="text-center mb-4">Forgot Password</h2>

      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label>Email</label>

          <input
            type="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <button className="btn btn-primary w-100">
          Send Reset Link
        </button>
      </form>
    </div>
  );
}

export default ForgotPassword;