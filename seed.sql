-- ============================================================
--  Davetly / HappyDay — Kategori Seed Verisi
--  Hedef: PostgreSQL (HappyDayDb)
--
--  Çalıştırma:
--    psql -U postgres -d HappyDayDb -f seed.sql
--  veya
--    docker exec -i happyday-postgres psql -U postgres -d HappyDayDb < seed.sql
--
--  Not: Idempotent (tekrar tekrar çalıştırılabilir), mevcut
--  kategorileri silmez veya güncellemez. Var olan kayıtlar
--  korunur, sadece eksik olanlar eklenir.
-- ============================================================

BEGIN;

INSERT INTO "Categories" ("Name")
SELECT c."Name"
FROM (
    VALUES
        ('Düğün Salonu'),
        ('Kır Bahçesi'),
        ('Kırık Ev'),
        ('Restaurant & Lounge'),
        ('Nikah Salonu'),
        ('Kongre & Toplantı Salonu'),
        ('Sosyal Tesis & Kulüp'),
        ('Rooftop & Teras'),
        ('Tekne & Yat'),
        ('Sanat Galerisi & Loft'),
        ('Otel & Balo Salonu'),
        ('Bahçe & Peyzaj')
) AS c("Name")
WHERE NOT EXISTS (
    SELECT 1 FROM "Categories" x WHERE x."Name" = c."Name"
);

COMMIT;

-- ============================================================
--  Kontrol
-- ============================================================
SELECT "Id", "Name" FROM "Categories" ORDER BY "Id";
