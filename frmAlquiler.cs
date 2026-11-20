using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace jason;

public class frmAlquiler : Form
{
    private const decimal RecargoSeguro = 300m;
    private const decimal RecargoGps = 100m;
    private const decimal CostoDomicilio = 500m;
    private const decimal PorcentajeDescuento = 0.15m;
    private const decimal PorcentajeItbis = 0.18m;
    private readonly CultureInfo cultura = CultureInfo.GetCultureInfo("en-US");
    private decimal subtotal = 0m;
    private int cantidadAlquileres = 0;

    private readonly TextBox txtCliente = new() { Name = "txtCliente" };
    private readonly TextBox txtVehiculo = new() { Name = "txtVehiculo" };
    private readonly TextBox txtTarifa = new() { Name = "txtTarifa" };
    private readonly DateTimePicker dtpSalida = new() { Name = "dtpSalida", Format = DateTimePickerFormat.Short };
    private readonly CheckBox chkCorporativo = new() { Name = "chkCorporativo", Text = "Cliente corporativo", AutoSize = true };
    private readonly NumericUpDown nudDias = new() { Name = "nudDias", Minimum = 1, Maximum = 30, Value = 1 };
    private readonly RadioButton rbtBasico = new() { Name = "rbtBasico", Text = "Básico", AutoSize = true, Checked = true };
    private readonly RadioButton rbtCompleto = new() { Name = "rbtCompleto", Text = "Completo (+RD$300)", AutoSize = true };
    private readonly RadioButton rbtSucursal = new() { Name = "rbtSucursal", Text = "En sucursal", AutoSize = true, Checked = true };
    private readonly RadioButton rbtDomicilio = new() { Name = "rbtDomicilio", Text = "Domicilio (+RD$500)", AutoSize = true };
    private readonly CheckBox chkGps = new() { Name = "chkGps", Text = "GPS (+RD$100 por día)", AutoSize = true };
    private readonly Label lblEncabezado = new() { Name = "lblEncabezado", AutoSize = false };
    private readonly ListBox lstAlquileres = new() { Name = "lstAlquileres", Font = new Font("Consolas", 9.2f), HorizontalScrollbar = true };
    private readonly Label lblSubtotal = CrearResultado("lblSubtotal");
    private readonly Label lblDescuento = CrearResultado("lblDescuento");
    private readonly Label lblDomicilio = CrearResultado("lblDomicilio");
    private readonly Label lblItbis = CrearResultado("lblItbis");
    private readonly Label lblTotal = CrearResultado("lblTotal");
    private readonly Button btnAgregar = new() { Name = "btnAgregar", Text = "Agregar al alquiler" };
    private readonly Button btnCalcular = new() { Name = "btnCalcular", Text = "Calcular total" };
    private readonly Button btnLimpiar = new() { Name = "btnLimpiar", Text = "Nuevo alquiler" };
    private readonly Button btnSalir = new() { Name = "btnSalir", Text = "Salir" };

    private static Label CrearResultado(string nombre) => new()
    {
        Name = nombre, TextAlign = ContentAlignment.MiddleRight,
        BackColor = Color.Transparent, Font = new Font("Segoe UI", 9, FontStyle.Regular)
    };

    public frmAlquiler()
    {
        Name = "frmAlquiler";
        Text = "RentaCar Express - Alquileres";
        ClientSize = new Size(1200, 750);
        MinimumSize = new Size(1216, 789);
        MaximumSize = new Size(1216, 789);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.FromArgb(240, 240, 240);
        AutoScaleMode = AutoScaleMode.None;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ConstruirInterfaz();
        Load += FrmAlquiler_Load;
        btnAgregar.Click += BtnAgregar_Click;
        btnCalcular.Click += BtnCalcular_Click;
        btnLimpiar.Click += (_, _) => NuevoAlquiler();
        btnSalir.Click += BtnSalir_Click;
    }

    private Label Etiqueta(string texto, int x, int y, int ancho = 90)
    {
        var label = new Label { Text = texto, Location = new Point(x, y), Size = new Size(ancho, 25), TextAlign = ContentAlignment.MiddleLeft };
        Controls.Add(label);
        return label;
    }

    private static void Ubicar(Control control, int x, int y, int ancho, int alto)
    {
        control.Location = new Point(x, y);
        control.Size = new Size(ancho, alto);
    }

    private void ConstruirInterfaz()
    {
        // Distribución igual a la captura proporcionada: datos a la izquierda,
        // listado y resultados a la derecha, botones inferiores.
        Etiqueta("Cliente:", 60, 45);
        Ubicar(txtCliente, 163, 45, 278, 27); Controls.Add(txtCliente);
        Ubicar(chkCorporativo, 55, 102, 250, 25); Controls.Add(chkCorporativo);
        Etiqueta("Salida:", 60, 158);
        Ubicar(dtpSalida, 163, 158, 342, 28); Controls.Add(dtpSalida);
        Etiqueta("Vehículo:", 60, 222);
        Ubicar(txtVehiculo, 163, 222, 278, 28); Controls.Add(txtVehiculo);
        Etiqueta("Tarifa:", 60, 287);
        Ubicar(txtTarifa, 163, 287, 278, 28); Controls.Add(txtTarifa);
        Etiqueta("Días:", 60, 351);
        Ubicar(nudDias, 163, 351, 165, 28); Controls.Add(nudDias);

        var grpSeguro = new GroupBox { Name = "grpSeguro", Text = "Seguro", Location = new Point(45, 426), Size = new Size(285, 144) };
        Ubicar(rbtBasico, 16, 29, 250, 26);
        Ubicar(rbtCompleto, 16, 81, 250, 26);
        grpSeguro.Controls.AddRange(new Control[] { rbtBasico, rbtCompleto }); Controls.Add(grpSeguro);
        var grpEntrega = new GroupBox { Name = "grpEntrega", Text = "Entrega", Location = new Point(351, 426), Size = new Size(285, 144) };
        Ubicar(rbtSucursal, 16, 29, 250, 26);
        Ubicar(rbtDomicilio, 16, 81, 250, 26);
        grpEntrega.Controls.AddRange(new Control[] { rbtSucursal, rbtDomicilio }); Controls.Add(grpEntrega);
        Ubicar(chkGps, 55, 590, 300, 28); Controls.Add(chkGps);

        Ubicar(btnAgregar, 752, 48, 351, 85); Controls.Add(btnAgregar);
        Ubicar(lblEncabezado, 723, 172, 417, 27);
        lblEncabezado.Font = new Font("Consolas", 10f, FontStyle.Bold);
        Controls.Add(lblEncabezado);
        Ubicar(lstAlquileres, 723, 201, 417, 238); Controls.Add(lstAlquileres);
        AgregarFila("Subtotal:", lblSubtotal, 478);
        AgregarFila("Descuento:", lblDescuento, 524);
        AgregarFila("Domicilio:", lblDomicilio, 570);
        AgregarFila("ITBIS (18 %):", lblItbis, 616);
        var etiquetaTotal = Etiqueta("TOTAL A PAGAR:", 727, 676, 200);
        etiquetaTotal.Font = new Font(Font, FontStyle.Bold);
        Ubicar(lblTotal, 1024, 676, 112, 27);
        lblTotal.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        Controls.Add(lblTotal);
        Ubicar(btnCalcular, 46, 681, 186, 61);
        Ubicar(btnLimpiar, 255, 681, 186, 61);
        Ubicar(btnSalir, 464, 681, 172, 61);
        Controls.AddRange(new Control[] { btnCalcular, btnLimpiar, btnSalir });
    }

    private void AgregarFila(string texto, Label resultado, int y)
    {
        Etiqueta(texto, 727, y, 160);
        Ubicar(resultado, 1020, y, 116, 27);
        Controls.Add(resultado);
    }

    private string Dinero(decimal monto) => "RD$ " + monto.ToString("N2", cultura);

    private void FrmAlquiler_Load(object? sender, EventArgs e) => NuevoAlquiler();

    private void ReiniciarVehiculo()
    {
        txtVehiculo.Clear(); txtTarifa.Clear(); nudDias.Value = 1;
        rbtBasico.Checked = true; chkGps.Checked = false;
        txtVehiculo.Focus();
    }

    private void NuevoAlquiler()
    {
        subtotal = 0m; cantidadAlquileres = 0;
        txtCliente.Clear(); chkCorporativo.Checked = false;
        dtpSalida.Value = DateTime.Today;
        rbtSucursal.Checked = true;
        lstAlquileres.Items.Clear();
        lblEncabezado.Text = "Días".PadRight(6) + "Vehículo".PadRight(24) + "Tarifa".PadLeft(10) + "Total".PadLeft(12);
        lblSubtotal.Text = Dinero(0); lblDescuento.Text = Dinero(0);
        lblDomicilio.Text = Dinero(0); lblItbis.Text = Dinero(0); lblTotal.Text = Dinero(0);
        ReiniciarVehiculo(); txtCliente.Focus();
    }

    private void BtnAgregar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCliente.Text))
        {
            MessageBox.Show("Ingrese el nombre del cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCliente.Focus(); return;
        }
        if (string.IsNullOrWhiteSpace(txtVehiculo.Text))
        {
            MessageBox.Show("Ingrese el nombre del vehículo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtVehiculo.Focus(); return;
        }
        string textoTarifa = txtTarifa.Text.Trim();
        if (!decimal.TryParse(textoTarifa, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal tarifa) &&
            !decimal.TryParse(textoTarifa, NumberStyles.Number, CultureInfo.InvariantCulture, out tarifa))
        {
            MessageBox.Show("Ingrese una tarifa numérica válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTarifa.Focus(); return;
        }
        if (tarifa <= 0)
        {
            MessageBox.Show("La tarifa debe ser mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTarifa.Focus(); return;
        }
        try
        {
            decimal tarifaDiaria = tarifa + (rbtCompleto.Checked ? RecargoSeguro : 0m) + (chkGps.Checked ? RecargoGps : 0m);
            decimal totalLinea = tarifaDiaria * nudDias.Value;
            string seguro = rbtCompleto.Checked ? "Completo" : "Básico";
            string nombreVehiculo = txtVehiculo.Text.Trim() + " (" + seguro + (chkGps.Checked ? ", GPS" : "") + ")";
            // Columnas solicitadas: 6, 24, 10 y 12 caracteres.
            string linea = ((int)nudDias.Value).ToString().PadRight(6)
                         + (nombreVehiculo.Length > 24 ? nombreVehiculo[..24] : nombreVehiculo).PadRight(24)
                         + tarifaDiaria.ToString("N2", cultura).PadLeft(10)
                         + totalLinea.ToString("N2", cultura).PadLeft(12);
            lstAlquileres.Items.Add(linea);
            subtotal += totalLinea;
            cantidadAlquileres++;
            lblSubtotal.Text = Dinero(subtotal);
            // Los resultados finales se calculan exclusivamente con Calcular total.
            lblDescuento.Text = Dinero(0); lblDomicilio.Text = Dinero(0);
            lblItbis.Text = Dinero(0); lblTotal.Text = Dinero(0);
            ReiniciarVehiculo();
        }
        catch (OverflowException)
        {
            MessageBox.Show("La tarifa ingresada es demasiado grande.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnCalcular_Click(object? sender, EventArgs e)
    {
        if (cantidadAlquileres == 0)
        {
            MessageBox.Show("Debe agregar al menos un vehículo antes de calcular.", "Sin vehículos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        decimal descuento = chkCorporativo.Checked ? subtotal * PorcentajeDescuento : 0m;
        decimal domicilio = rbtDomicilio.Checked ? CostoDomicilio : 0m;
        decimal baseImponible = subtotal - descuento + domicilio;
        decimal itbis = baseImponible * PorcentajeItbis;
        decimal total = baseImponible + itbis;
        lblSubtotal.Text = Dinero(subtotal);
        lblDescuento.Text = Dinero(descuento);
        lblDomicilio.Text = Dinero(domicilio);
        lblItbis.Text = Dinero(itbis);
        lblTotal.Text = Dinero(total);
    }

    private void BtnSalir_Click(object? sender, EventArgs e)
    {
        var respuesta = MessageBox.Show("¿Desea salir del programa?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (respuesta == DialogResult.Yes) Close();
    }
}
