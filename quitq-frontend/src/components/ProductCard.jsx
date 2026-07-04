import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";

function ProductCard({ product }) {
  return (
    <Card className="shadow-sm border-0 h-100">

      <Link
        to={`/products/${product.productId}`}
        className="text-decoration-none text-dark"
      >

        <Card.Img
          variant="top"
          src={product.imageUrl}
          onError={(e) => {
            e.target.src =
              "https://picsum.photos/300/300";
          }}
          style={{
            height: "220px",
            objectFit: "contain",
            padding: "15px"
          }}
        />

        <Card.Body>

          <Card.Title>
            {product.productName}
          </Card.Title>

          <Card.Text className="text-success fw-bold">
            ₹{product.price}
          </Card.Text>

        </Card.Body>

      </Link>

    </Card>
  );
}

export default ProductCard;