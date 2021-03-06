﻿using SGE.AccesoDatos.Ventas;
using SGE.Entidades.Administracion;
using SGE.Entidades.Ventas;
using SGE.Negocios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Negocios.Ventas
{
    public class blCotizacion : blBase
    {
        public blCotizacion(Sesion sesion) { base.sesion = sesion; }

        daCotizacion daCotizacion;
        daCotizacionGrupo daCotizacionGrupo;
        daCotizacionItem daCotizacionItem;
        daCotizacionServicio daCotizacionServicio;

        public object[] ObtenerTodos(Paginacion paginacion, Orden orden)
        {
            object[] datos;
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.AbrirSesion();
                datos = daCotizacion.ObtenerPaginacion(new List<object[]>(), paginacion, orden);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return datos;
        }

        public List<Cotizacion> ObtenerPendientes(int[] idsExcluir)
        {
            List<Cotizacion> cotizaciones;
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.AbrirSesion();
                List<object[]> filtros = new List<object[]>();
                filtros.Add(new object[] { "estado", 0 });
                filtros.Add(new object[] { "idCotizacion", idsExcluir, "NOT_IN" });
                cotizaciones = daCotizacion.ObtenerLista(filtros);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return cotizaciones;
        }

        public Cotizacion ObtenerPorId(int idCotizacion)
        {
            Cotizacion cotizacion;
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.AbrirSesion();
                cotizacion = daCotizacion.ObtenerPorId(idCotizacion);
                daCotizacionGrupo = new daCotizacionGrupo();
                daCotizacionGrupo.AsignarSesion(daCotizacion);
                List<object[]> filtros = new List<object[]>();
                filtros.Add(new object[] { "idCotizacion", idCotizacion });
                cotizacion.grupos = daCotizacionGrupo.ObtenerLista(filtros);
                daCotizacionItem = new daCotizacionItem();
                daCotizacionItem.AsignarSesion(daCotizacion);
                daCotizacionServicio = new daCotizacionServicio();
                daCotizacionServicio.AsignarSesion(daCotizacion);
                foreach (CotizacionGrupo grupo in cotizacion.grupos)
                {
                    filtros = new List<object[]>();
                    filtros.Add(new object[] { "idCotizacionGrupo", grupo.idCotizacionGrupo });
                    grupo.items = daCotizacionItem.ObtenerLista(filtros);
                    foreach (CotizacionItem item in grupo.items)
                    {
                        filtros = new List<object[]>();
                        filtros.Add(new object[] { "idCotizacionItem", item.idCotizacionItem });
                        item.servicios = daCotizacionServicio.ObtenerLista(filtros);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return cotizacion;
        }

        public bool Agregar(Cotizacion cotizacion)
        {
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.IniciarTransaccion();
                if (string.IsNullOrEmpty(cotizacion.numero))
                {
                    cotizacion.numero = generarNumeracion(daCotizacion, cotizacion.numeracion.idNumeracion);
                }
                cotizacion.fechaCreacion = DateTime.Now;
                daCotizacion.Agregar(cotizacion);
                daCotizacionGrupo = new daCotizacionGrupo();
                daCotizacionGrupo.AsignarSesion(daCotizacion);
                daCotizacionItem = new daCotizacionItem();
                daCotizacionItem.AsignarSesion(daCotizacion);
                daCotizacionServicio = new daCotizacionServicio();
                daCotizacionServicio.AsignarSesion(daCotizacion);
                foreach (CotizacionGrupo grupo in cotizacion.grupos)
                {
                    grupo.idCotizacion = cotizacion.idCotizacion;
                    daCotizacionGrupo.Agregar(grupo);
                    foreach (CotizacionItem item in grupo.items)
                    {
                        item.idCotizacionGrupo = grupo.idCotizacionGrupo;
                        daCotizacionItem.Agregar(item);
                        foreach (CotizacionServicio servicio in item.servicios)
                        {
                            servicio.idCotizacionItem = item.idCotizacionItem;
                            daCotizacionServicio.Agregar(servicio);
                        }
                    }
                }
                daCotizacion.ConfirmarTransaccion();
            }
            catch (Exception)
            {
                daCotizacion.AbortarTransaccion();
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return true;
        }

        public bool Actualizar(Cotizacion cotizacion)
        {
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.IniciarTransaccion();
                Cotizacion cotizacion_ = daCotizacion.ObtenerPorId(cotizacion.idCotizacion);
                cotizacion_.descripcion = cotizacion.descripcion;
                cotizacion_.cliente = cotizacion.cliente;
                cotizacion_.cotizador = cotizacion.cotizador;
                cotizacion_.lpMaterial = cotizacion.lpMaterial;
                cotizacion_.lpServicio = cotizacion.lpServicio;
                cotizacion_.lpMaquina = cotizacion.lpMaquina;
                cotizacion_.moneda = cotizacion.moneda;
                cotizacion_.vendedor = cotizacion.vendedor;
                cotizacion_.formaPago = cotizacion.formaPago;
                cotizacion_.contacto = cotizacion.contacto;
                cotizacion_.observacion = cotizacion.observacion;
                cotizacion_.pcjUtilidad = cotizacion.pcjUtilidad;
                cotizacion_.monUtilidad = cotizacion.monUtilidad;
                cotizacion_.subTotal = cotizacion.subTotal;
                cotizacion_.total = cotizacion.total;
                daCotizacionGrupo = new daCotizacionGrupo();
                daCotizacionGrupo.AsignarSesion(daCotizacion);
                daCotizacionItem = new daCotizacionItem();
                daCotizacionItem.AsignarSesion(daCotizacion);
                daCotizacionServicio = new daCotizacionServicio();
                daCotizacionServicio.AsignarSesion(daCotizacion);
                foreach (CotizacionGrupo grupo in cotizacion.grupos)
                {
                    if (grupo.idCotizacionGrupo == 0)
                    {
                        grupo.idCotizacion = cotizacion.idCotizacion;
                        daCotizacionGrupo.Agregar(grupo);
                        foreach (CotizacionItem item in grupo.items)
                        {
                            item.idCotizacionGrupo = grupo.idCotizacionGrupo;
                            daCotizacionItem.Agregar(item);
                            foreach (CotizacionServicio servicio in item.servicios)
                            {
                                servicio.idCotizacionItem = item.idCotizacionItem;
                                daCotizacionServicio.Agregar(servicio);
                            }
                        }
                    }
                    else
                    {
                        CotizacionGrupo grupo_ = daCotizacionGrupo.ObtenerPorId(grupo.idCotizacionGrupo);
                        grupo_.titulo = grupo.titulo;
                        grupo_.cantidad = grupo.cantidad;
                        foreach (CotizacionItem item in grupo.items)
                        {
                            if (item.idCotizacionItem == 0)
                            {
                                item.idCotizacionGrupo = grupo.idCotizacionGrupo;
                                daCotizacionItem.Agregar(item);
                                foreach (CotizacionServicio servicio in item.servicios)
                                {
                                    servicio.idCotizacionItem = item.idCotizacionItem;
                                    daCotizacionServicio.Agregar(servicio);
                                }
                            }
                            else
                            {
                                CotizacionItem item_ = daCotizacionItem.ObtenerPorId(item.idCotizacionItem);
                                item_.titulo = item.titulo;
                                item_.servicio = item.servicio;
                                item_.maquina = item.maquina;
                                item_.material = item.material;
                                item_.flagMA = item.flagMA;
                                item_.flagMC = item.flagMC;
                                item_.flagTYR = item.flagTYR;
                                item_.flagGRF = item.flagGRF;
                                item_.flagMAT = item.flagMAT;
                                item_.flagSRV = item.flagSRV;
                                item_.flagFND = item.flagFND;
                                item_.valXMA = item.valXMA;
                                item_.valYMA = item.valYMA;
                                item_.valXMC = item.valXMC;
                                item_.valYMC = item.valYMC;
                                item_.valTC = item.valTC;
                                item_.valRT = item.valRT;
                                item_.valFND = item.valFND;
                                item_.valXFI = item.valXFI;
                                item_.valYFI = item.valYFI;
                                item_.valSEPX = item.valSEPX;
                                item_.valSEPY = item.valSEPY;
                                item_.valPLG = item.valPLG;
                                item_.flagGPR = item.flagGPR;
                                item_.flagGIR = item.flagGIR;
                                item_.valPZSP = item.valPZSP;
                                item_.valPZSI = item.valPZSI;
                                item_.metodoImpresion = item.metodoImpresion;
                                item_.valMAT = item.valMAT;
                                item_.valDEM = item.valDEM;
                                item_.valPRD = item.valPRD;
                                item_.valCNT = item.valCNT;
                                item_.valPGS = item.valPGS;
                                item_.observacion = item.observacion;
                                item_.flagINCP = item.flagINCP;
                                item_.flagPRECP = item.flagPRECP;
                                item_.valTLMAQ = item.valTLMAQ;
                                item_.valTLMAT = item.valTLMAT;
                                item_.valTLSRV = item.valTLSRV;
                                item_.valTOTAL = item.valTOTAL;
                                foreach (CotizacionServicio servicio in item.servicios)
                                {
                                    if (servicio.idCotizacionServicio == 0)
                                    {
                                        servicio.idCotizacionItem = item.idCotizacionItem;
                                        daCotizacionServicio.Agregar(servicio);
                                    }
                                    else
                                    {
                                        CotizacionServicio servicio_ = daCotizacionServicio.ObtenerPorId(servicio.idCotizacionServicio);
                                        servicio_.cantidad = servicio.cantidad;
                                        servicio_.precio = servicio.precio;
                                        servicio_.precioM = servicio.precioM;
                                        servicio_.unidad = servicio.unidad;
                                        servicio_.total = servicio.total;
                                    }
                                }
                                foreach (int idServicio in item.idsServicios)
                                {
                                    daCotizacionServicio.EliminarPorId(idServicio, constantes.esquemas.Ventas);
                                }
                            }
                        }
                        foreach (int idItem in grupo.idsItems)
                        {
                            daCotizacionItem.EliminarPorId(idItem, constantes.esquemas.Ventas);
                        }
                    }
                }
                foreach (int idGrupo in cotizacion.idsGrupos)
                {
                    daCotizacionGrupo.EliminarPorId(idGrupo, constantes.esquemas.Ventas);
                    daCotizacionItem.EliminarPorIdCotizacionGrupo(idGrupo);
                }
                daCotizacion.ConfirmarTransaccion();
            }
            catch (Exception)
            {
                daCotizacion.AbortarTransaccion();
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return true;
        }

        public bool Eliminar(List<int> ids)
        {
            try
            {
                daCotizacion = new daCotizacion();
                daCotizacion.IniciarTransaccion();
                foreach (var idCotizacion in ids)
                {
                    daCotizacion.EliminarPorId(idCotizacion, constantes.esquemas.Ventas);
                    daCotizacionGrupo = new daCotizacionGrupo();
                    daCotizacionGrupo.AsignarSesion(daCotizacion);
                    List<object[]> filtros = new List<object[]>();
                    filtros.Add(new object[] { "idCotizacion", idCotizacion });
                    List<CotizacionGrupo> grupos = daCotizacionGrupo.ObtenerLista(filtros);
                    daCotizacionGrupo.EliminarPorIdCotizacion(idCotizacion);
                    daCotizacionItem = new daCotizacionItem();
                    daCotizacionItem.AsignarSesion(daCotizacion);
                    foreach (CotizacionGrupo grupo in grupos)
                    {
                        filtros = new List<object[]>();
                        filtros.Add(new object[] { "idCotizacionGrupo", grupo.idCotizacionGrupo });
                        List<CotizacionItem> items = daCotizacionItem.ObtenerLista(filtros);
                        daCotizacionItem.EliminarPorIdCotizacionGrupo(grupo.idCotizacionGrupo);
                        daCotizacionServicio = new daCotizacionServicio();
                        daCotizacionServicio.AsignarSesion(daCotizacion);
                        foreach (CotizacionItem item in items)
                        {
                            daCotizacionServicio.EliminarPorIdCotizacionItem(item.idCotizacionItem);
                        }
                    }
                }
                daCotizacion.ConfirmarTransaccion();
            }
            catch (Exception)
            {
                daCotizacion.AbortarTransaccion();
                throw;
            }
            finally
            {
                daCotizacion.CerrarSesion();
            }
            return true;
        }
    }
}
