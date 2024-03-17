import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
// import { addItem, delItem } from "../redux/action";
import { NavLink, useParams } from "react-router-dom";
import Skeleton from "react-loading-skeleton";
import axios from "axios";
import ReactModal from "react-modal";
import ReactPlayer from "react-player";

const Product = () => {
  //   const [cartBtn, setCartBtn] = useState("Add to Cart");w

  const { id } = useParams();
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const handleModalOpen = () => setShowModal(true);
  const handleModalClose = () => setShowModal(false);
  //   const dispatch = useDispatch();

  //hover anh
  const [isHovered, setIsHovered] = useState(false);
  const handleMouseEnter = () => {
    setIsHovered(true);
  };
  const handleMouseLeave = () => {
    setIsHovered(false);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await axios.get(
          `https://localhost:7016/api/getMovieById?Id=${id}`
        );

        setData(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);

  const Loading = () => {
    return (
      <>
        <div className="col-md-6">
          <Skeleton height={400} />
        </div>
        <div className="col-md-6">
          <Skeleton height={50} width={300} />
          <Skeleton height={75} />
          <Skeleton height={25} width={150} />
          <Skeleton height={50} />
          <Skeleton height={150} />
          <Skeleton height={50} width={100} />
          <Skeleton height={50} width={100} style={{ marginLeft: 6 }} />
        </div>
      </>
    );
  };

  //   const handleCart = (product) => {
  //     if (cartBtn === "Add to Cart") {
  //       dispatch(addItem(product));
  //       setCartBtn("Remove from Cart");
  //     } else {
  //       dispatch(delItem(product));
  //       setCartBtn("Add to Cart");
  //     }
  //   };

  const ShowProduct = () => {
    return (
      <>
        <div key={data.Id} className="col-md-6">
          <img
            // onClick={handleModalOpen}
            src={data.Image}
            alt={data.Name}
            height="400px"
            width="400px"
            // style={{ cursor: "pointer" }}
            onMouseEnter={handleMouseEnter}
            onMouseLeave={handleMouseLeave}
          />
          {isHovered && (
            <div
              style={{
                position: "absolute",
                top: "48%",
                left: "19%",
                // transform: "translate(-50%, -50%)",
              }}
            >
              <img
                onClick={handleModalOpen}
                src="https://icon-library.com/images/play-button-icon/play-button-icon-16.jpg"
                width="50px"
                height="50px"
                style={{
                  backgroundColor: "white",
                  borderRadius: "100%",
                  cursor: "pointer",
                }}
              />
            </div>
          )}
          {showModal && (
            <ReactModal
              isOpen={true}
              onClose={handleModalClose}
              style={{
                overlay: {
                  backgroundColor: "rgba(0, 0, 0, 0.75)",
                },
                content: {
                  width: "60%",
                  height: "60%",
                  margin: "0 auto",
                  borderRadius: "5px",
                  padding: "40px",
                  marginTop: "70px",
                },
              }}
            >
              <button onClick={handleModalClose}>x</button>
              <ReactPlayer url={data.Trailer} />
            </ReactModal>
          )}
        </div>
        <div className="col-md-6">
          <h4 className="text-uppercase text-black-50">{data.category}</h4>
          <h1 className="display-5">{data.Name}</h1>
          <p className="lead fw-bolder">
            Rating {5}
            <i className="fa fa-star"></i>
          </p>
          <h3 className="display-6 fw-bold my-4">Giá vé: 90.000 VND</h3>
          <p className="lead">Đạo diễn: {data.Director}</p>
          <p className="lead">{data.Description}</p>
          <button
            // onClick={() => handleCart(data)}
            className="btn btn-outline-dark mx-4 py-2"
          >
            {/* {cartBtn} */}
            Mua vé
          </button>
          <NavLink to="/cart" className="btn btn-dark ms-2 px-3 py-2">
            Huỷ vé
          </NavLink>
        </div>
      </>
    );
  };

  return (
    <div>
      <div className="container py-5">
        <div className="row py-4">
          {loading ? <Loading /> : <ShowProduct />}
        </div>
      </div>
    </div>
  );
};

export default Product;
