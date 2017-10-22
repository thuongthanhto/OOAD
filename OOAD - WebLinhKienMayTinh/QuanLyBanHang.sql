create database QuanLyBanHang
go
use QuanLyBanHang
go 
create table NhaSanXuat
(
	MaNSX int primary key identity(1,1),
	TenNSX nvarchar(100),
	ThongTin nvarchar(255),
	Logo nvarchar(MAX)
)
go
create table NhaCungCap
(
	MaNCC int primary key identity(1,1),
	TenNCC nvarchar(100),
	DiaChi nvarchar(255),
	Email nvarchar(255),
	SoDienThoai varchar(12),
	Fax nvarchar(50)
)
go 
create table PhieuNhap
(
	MaPN int primary key identity(1,1),
	MaNCC int,
	NgayNhap datetime,
	DaXoa bit,
	
	constraint FK_PhieuNhap_NhaCungCap foreign key(MaNCC) references NhaCungCap(MaNCC) on delete cascade
)
go
create table LoaiThanhVien
(
	MaLoaiTV int primary key identity(1,1),
	TenLoai nvarchar(50),
	UuDai int
)
go 
create table ThanhVien
(
	MaThanhVien int primary key identity(1,1),
	TaiKhoan nvarchar(100),
	MatKhau nvarchar(100),
	HoTen nvarchar(100),
	DiaChi nvarchar(255),
	Email nvarchar(255),
	SoDienThoai varchar(12), 
	CauHoi nvarchar(MAX),
	CauTraLoi nvarchar(MAX),
	MaLoaiTV int,	

	constraint FK_ThanhVien_LoaiThanhVien foreign key(MaLoaiTV) references LoaiThanhVien(MaLoaiTV) on delete cascade
)
go
create table KhachHang
(
	MaKH int primary key identity(1,1),
	TenKH nvarchar(100),
	DiaChi nvarchar(100),
	Email nvarchar(255),
	SoDienThoai varchar(12),
	MaThanhVien int,

	constraint FK_KhachHang_ThanhVien foreign key(MaThanhVien) references ThanhVien(MaThanhVien) on delete cascade
)
go
create table LoaiSanPham
(
	MaLoaiSP int primary key identity(1,1),
	TenLoai nvarchar(100),
	Icon nvarchar(MAX),
	BiDanh nvarchar(50)
)
go
create table SanPham
(
	MaSP int primary key identity(1,1),
	TenSP nvarchar(255),
	DonGia decimal(18,0),
	NgayCapNhat datetime,
	CauHinh nvarchar(MAX),
	MoTa nvarchar(MAX),
	HinhAnh nvarchar(MAX),
	SoLuongTon int, 
	LuotXem int,
	LuotBinhChon int,
	LuotBinhLuan int,
	SoLanMua int, 
	Moi int,
	MaNCC int, 
	MaNSX int, 
	MaLoaiSP int, 
	DaXoa bit,

	constraint FK_SanPham_LoaiSanPham foreign key(MaLoaiSP) references LoaiSanPham(MaLoaiSP),
	constraint FK_SanPham_NhaCungCap foreign key(MaNCC) references NhaCungCap(MaNCC),
	constraint FK_SanPham_NhaSanXuat foreign key(MaNSX) references NhaSanXuat(MaNSX)
)
go
create table ChiTietPhieuNhap
(
	MaChiTietPN int primary key identity(1,1),
	MaPN int,
	MaSP int,
	DonGiaNhap decimal(18,0),
	SoLuongNhap int,

	constraint FK_ChiTietPhieuNhap_SanPham foreign key(MaSP) references SanPham(MaSP),
	constraint FK_ChiTietPhieuNhap_PhieuNhap foreign key(MaPN) references PhieuNhap(MaPN)
)
go
create table DonDatHang
(
	MaDDH int primary key identity(1,1), 
	NgayDat datetime, 
	TinhTrangGiaoHang bit, 
	NgayGiao datetime, 
	DaThanhToan bit, 
	MaKH int, 
	UuDai int,
	DaHuy int, 
	DaXoa int,

	constraint FK_DonDatHang_KhachHang foreign key(MaKH) references KhachHang(MaKH) on delete cascade
)
go
create table ChiTietDonDatHang
(
	MaChiTietDDH int primary key identity(1,1),
	MaDDH int, 
	MaSP int, 
	TenSP nvarchar(255),
	SoLuong int, 
	DonGia decimal(18,0),
	
	constraint FK_ChiTietDonDatHang_DonDatHang foreign key(MaDDH) references DonDatHang(MaDDH),
	constraint FK_ChiTietDonDatHang_SanPham foreign key(MaSP) references SanPham(MaSP)
)
go
create table BinhLuan
(
	MaBL int primary key identity(1,1),
	NoiDungBL nvarchar(MAX),
	MaThanhVien int, 
	MaSP int, 

	constraint FK_BinhLuan_ThanhVien foreign key(MaThanhVien) references ThanhVien(MaThanhVien),
	constraint FK_BinhLuan_SanPham foreign key(MaSP) references SanPham(MaSP)
)
go
create table Quyen 
(
    MaQuyen  nvarchar(50) primary key,
    TenQuyen nvarchar(50)
)
go
create table LoaiThanhVien_Quyen
 (
    MaLoaiTV int not null,
    MaQuyen  nvarchar(50) not null,
    GhiChu  nvarchar(50),
    primary key(MaLoaiTV, MaQuyen),
    constraint FK_LoaiThanhVien_Quyen_LoaiThanhVien foreign key (MaLoaiTV) references LoaiThanhVien(MaLoaiTV),
    constraint FK_LoaiThanhVien_Quyen_Quyen foreign key (MaQuyen) references Quyen(MaQuyen)
);
