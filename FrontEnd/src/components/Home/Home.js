import home1 from "../../assets/home1.jpg";
import home2 from "../../assets/home2.png";
import Carousel from "react-bootstrap/Carousel";
import Products from "../Products/Products";

const Home = () => {
  return (
    <div>
      <Carousel>
        <Carousel.Item>
          <img src={home2} class="card-img" alt="Background" height="625px" />
        </Carousel.Item>
        <Carousel.Item>
          <img src={home1} class="card-img mt-3" alt="Background" height="600px" />
          <Carousel.Caption></Carousel.Caption>
        </Carousel.Item>
      </Carousel>
      <Products />
    </div>

    
  );
};

export default Home;
