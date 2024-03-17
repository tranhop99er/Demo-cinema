import React, { useState, useEffect } from "react";
import Skeleton from "react-loading-skeleton";
import { NavLink } from "react-router-dom";
import axios from "axios";

const Products = () => {
  const [data, setData] = useState([]);
  // const [filter, setFilter] = useState(data);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await axios.get("https://localhost:7016/api/getMovie");

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
        <div className="col-md-3">
          <Skeleton height={350} />
        </div>
        <div className="col-md-3">
          <Skeleton height={350} />
        </div>
        <div className="col-md-3">
          <Skeleton height={350} />
        </div>
        <div className="col-md-3">
          <Skeleton height={350} />
        </div>
      </>
    );
  };

  // const filterProduct = (category) => {
  //   const updateList = data.filter((x) => {
  //     return x.category === category;
  //   });
  //   console.log(updateList);
  //   return setFilter(updateList);
  // };

  const ShowProducts = () => {
    return (
      <>
        {data.map((movie) => {
          return (
            <div className="col-md-3 mb-4">
              <div class="card h-100 text-center p-4" key={movie.Id}>
                <img
                  src={movie.Image}
                  class="card-img-top"
                  alt={movie.Name}
                  height="310px"
                  style={{ borderRadius: "25px" }}
                />

                <div class="card-body">
                  {movie.Name.length > 15 ? (
                    <h5 class="card-title mb-0">
                      {movie.Name.substring(0, 15)}...
                    </h5>
                  ) : (
                    <h5 class="card-title mb-0">{movie.Name}</h5>
                  )}

                  {/* <p class="card-text lead fw-bold">${movie.Name}</p> */}
                  <NavLink
                    to={`/products/${movie.Id}`}
                    className="btn btn-dark ms-2 px-3 mt-3"
                  >
                    Mua vé
                  </NavLink>
                </div>
              </div>
            </div>
          );
        })}
      </>
    );
  };

  return (
    <div>
      <div className="container my-5 py-5">
        <div className="row">
          <div className="col-12 mb-5">
            <h1 className="display-6 fw-bolder text-center">PHIM NỔI BẬT</h1>
            <hr />
          </div>
        </div>

        <div className="row justify-content-center">
          {loading ? <Loading /> : <ShowProducts />}
        </div>
      </div>
    </div>
  );
};

export default Products;
