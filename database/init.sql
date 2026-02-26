CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 1. COMPANIES
CREATE TABLE companies (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    image_url TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 2. CATEGORIES
CREATE TABLE categories (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    name TEXT NOT NULL,
    description TEXT
);

-- 3. UNITS
CREATE TABLE units (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    code TEXT NOT NULL,
    name TEXT NOT NULL
);

-- 4. WAREHOUSES
CREATE TABLE warehouses (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    name TEXT NOT NULL,
    location TEXT,
    is_active BOOLEAN NOT NULL DEFAULT true
);

-- 5. SUPPLIERS
CREATE TABLE suppliers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    name TEXT NOT NULL,
    contact_info TEXT,
    is_active BOOLEAN NOT NULL DEFAULT true
);

-- 6. PRODUCTS
CREATE TABLE products (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    category_id UUID NOT NULL REFERENCES categories(id),
    unit_id UUID NOT NULL REFERENCES units(id),
    supplier_id UUID REFERENCES suppliers(id),
    code TEXT,
    name TEXT NOT NULL,
    status TEXT NOT NULL DEFAULT 'Activo',
    image_url TEXT,
    min_stock_alert DECIMAL(10, 2) NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 7. PRODUCT_STOCKS
CREATE TABLE product_stocks (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    product_id UUID NOT NULL REFERENCES products(id),
    warehouse_id UUID NOT NULL REFERENCES warehouses(id),
    current_quantity DECIMAL(10, 2) NOT NULL DEFAULT 0,
    last_updated TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 8. INVENTORY_DOCUMENTS
CREATE TABLE inventory_documents (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    warehouse_id UUID NOT NULL REFERENCES warehouses(id),
    type TEXT NOT NULL, -- ENTRADA, SALIDA, AJUSTE
    status TEXT NOT NULL, -- BORRADOR, CONFIRMADO, CANCELADO
    document_date TIMESTAMP WITH TIME ZONE NOT NULL,
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 9. INVENTORY_DOCUMENT_LINES
CREATE TABLE inventory_document_lines (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    document_id UUID NOT NULL REFERENCES inventory_documents(id),
    product_id UUID NOT NULL REFERENCES products(id),
    quantity DECIMAL(10, 2) NOT NULL
);

-- 10. KARDEX_MOVEMENTS
CREATE TABLE kardex_movements (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    company_id UUID NOT NULL REFERENCES companies(id),
    product_id UUID NOT NULL REFERENCES products(id),
    warehouse_id UUID NOT NULL REFERENCES warehouses(id),
    document_id UUID REFERENCES inventory_documents(id),
    movement_type TEXT NOT NULL, -- IN, OUT
    quantity DECIMAL(10, 2) NOT NULL,
    balance DECIMAL(10, 2) NOT NULL,
    reason TEXT,
    movement_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================
-- SEED DATA (Mock data for testing)
-- ==========================================

DO $$
DECLARE
    v_company_id UUID := '11111111-1111-1111-1111-111111111111'::uuid;
    v_cat_elec UUID := '22222222-2222-2222-2222-222222222221'::uuid;
    v_cat_acce UUID := '22222222-2222-2222-2222-222222222222'::uuid;
    v_unit_und UUID := '33333333-3333-3333-3333-333333333331'::uuid;
    v_unit_kg UUID := '33333333-3333-3333-3333-333333333332'::uuid;
    v_wh_main UUID := '44444444-4444-4444-4444-444444444441'::uuid;
    v_supplier UUID := '55555555-5555-5555-5555-555555555551'::uuid;
    v_prod_lap UUID := '66666666-6666-6666-6666-666666666661'::uuid;
    v_prod_mou UUID := '66666666-6666-6666-6666-666666666662'::uuid;
    v_doc_id UUID := '77777777-7777-7777-7777-777777777771'::uuid;
BEGIN
    -- 1. Insert Company
    INSERT INTO companies (id, name, is_active) VALUES (v_company_id, 'Empresa Demo S.A.', true);

    -- 2. Insert Categories
    INSERT INTO categories (id, company_id, name, description) VALUES 
    (v_cat_elec, v_company_id, 'Electrónica', 'Productos de tecnología y computación'),
    (v_cat_acce, v_company_id, 'Accesorios', 'Accesorios varios para oficina');

    -- 3. Insert Units
    INSERT INTO units (id, company_id, code, name) VALUES 
    (v_unit_und, v_company_id, 'UND', 'Unidad'),
    (v_unit_kg, v_company_id, 'KG', 'Kilogramo');

    -- 4. Insert Warehouses
    INSERT INTO warehouses (id, company_id, name, location, is_active) VALUES 
    (v_wh_main, v_company_id, 'Almacén Principal', 'Zona Sur', true);

    -- 5. Insert Suppliers
    INSERT INTO suppliers (id, company_id, name, contact_info, is_active) VALUES 
    (v_supplier, v_company_id, 'Distribuidora Tecnológica', 'ventas@distribuidora.com', true);

    -- 6. Insert Products
    INSERT INTO products (id, company_id, category_id, unit_id, supplier_id, code, name, min_stock_alert) VALUES 
    (v_prod_lap, v_company_id, v_cat_elec, v_unit_und, v_supplier, 'LAP-001', 'Laptop ThinkPad T14', 5),
    (v_prod_mou, v_company_id, v_cat_acce, v_unit_und, v_supplier, 'MOU-002', 'Mouse Inalámbrico Logitech', 15);

    -- 7. Insert Initial Stock (Snapshot)
    INSERT INTO product_stocks (company_id, product_id, warehouse_id, current_quantity) VALUES 
    (v_company_id, v_prod_lap, v_wh_main, 10),
    (v_company_id, v_prod_mou, v_wh_main, 50);

    -- 8. Insert a confirmed INVENTORY DOCUMENT (Initial Data Load)
    INSERT INTO inventory_documents (id, company_id, warehouse_id, type, status, document_date, notes) VALUES 
    (v_doc_id, v_company_id, v_wh_main, 'ENTRADA', 'CONFIRMADO', CURRENT_TIMESTAMP, 'Carga inicial de inventario');

    -- 9. Insert Lines
    INSERT INTO inventory_document_lines (document_id, product_id, quantity) VALUES 
    (v_doc_id, v_prod_lap, 10),
    (v_doc_id, v_prod_mou, 50);

    -- 10. Insert Kardex Movements
    INSERT INTO kardex_movements (company_id, product_id, warehouse_id, document_id, movement_type, quantity, balance, reason) VALUES 
    (v_company_id, v_prod_lap, v_wh_main, v_doc_id, 'IN', 10, 10, 'Carga inicial'),
    (v_company_id, v_prod_mou, v_wh_main, v_doc_id, 'IN', 50, 50, 'Carga inicial');
END $$;
