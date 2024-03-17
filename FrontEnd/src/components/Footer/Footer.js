import betaLogo from "../../assets/betaLogo.jpg";

const Footer = () => {
  return (
    <footer class="footer mt-auto py-3 ">
      <div class="container">
        <div class="row">
          <div class="col-md-3">
            <ul>
              <li class="list-group-item pb-2">
                <img
                  src={betaLogo}
                  alt="logo"
                  style={{ width: "50%", height: "40%", objectFit: "cover" }}
                />
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Giới thiệu
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Liên hệ
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  F.A.Q
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Hoạt động xã hội
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Chính sách thanh toán, đổi trả - hoàn về
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Liên hệ quảng cáo
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Điều khoản bảo mật
                </a>
              </li>
              <li class="list-group-item">
                <a class="nav-item nav-link" href="#">
                  Hướng dẫn đặt vé online
                </a>
              </li>
            </ul>
          </div>

          <div class="col-md-9 row">
            <div class="col-md-5">
              <ul>
                <li class="list-group-item pb-2">
                  <h4 class="mb-4">CỤM RẠP BETA</h4>
                </li>
                <li class="list-group-item pb-2">
                  <a class="nav-item nav-link" href="#">
                    <i class="fa fa-angle-right"></i> Beta Cinemas Lào Cai -{" "}
                    <span class="hotline">Hotline 0358 968 970</span>
                  </a>
                </li>
                <li class="list-group-item pb-2">
                  <a class="nav-item nav-link" href="#">
                    <i class="fa fa-angle-right"></i> Beta Cinemas Trần Quang
                    Khải, TP Hồ Chí Minh -{" "}
                    <span class="hotline">Hotline 1900 638 362</span>
                  </a>
                </li>
                <li class="list-group-item"></li>
              </ul>
            </div>

            <div class="col-md-3">
              <h5>Kết nối với chúng tôi</h5>
              <ul class="list-unstyled">
                <li>Beta Cinemas Location 1 - Hotline</li>
              </ul>
            </div>

            <div class="col-md-4">
              <h5>Liên hệ</h5>
              <h6>CÔNG TY CỔ PHẦN BETA MEDIA</h6>
              <p>
                Giấy chứng nhận ĐKKD số: 0106633482 - Đăng ký lần đầu ngày
                08/09/2014 tại Sở Kế hoạch và Đầu tư Thành phố Hà Nội
              </p>
              <p>
                Địa chỉ trụ sở: Tầng 3, số 595, đường Giải Phóng, phường Giáp
                Bát, quận Hoàng Mai, thành phố Hà Nội
              </p>

              <p>Hotline: 1900 636807</p>

              <p>Email: cskh@betacorp.vn</p>
              <h6>Liên hệ hợp tác kinh doanh:</h6>
              <h6>Email: bachtx@betagroup.vn</h6>
              <h6>Điện thoại: 081 809 1118</h6>

              <div>
                <a href="#">
                  <i class="fab fa-facebook-square"></i>
                </a>
                <a href="#">
                  <i class="fab fa-youtube-square"></i>
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
