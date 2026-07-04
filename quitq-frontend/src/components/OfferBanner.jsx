import { useNavigate } from "react-router-dom";
function OfferBanner() {
  const navigate = useNavigate();
  return (
    <div
      className="container my-5 p-5 text-center text-white rounded"
      style={{
        background:
          "linear-gradient(90deg, #2874f0, #0d6efd)"
      }}
    >
      <h1 className="fw-bold">
        Big Savings Sale
      </h1>

      <h4 className="mt-3">
        Up to 80% OFF on Electronics & Fashion
      </h4>

     <button
  className="btn btn-warning"
  onClick={() =>
    navigate("/products")
  }
>
  Shop Now
</button>
    </div>
  );
}

export default OfferBanner;